using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using LuaBridge.Core.Abstract;
using LuaBridge.Core.Events;
using LuaBridge.Core.Events.Core;
using LuaBridge.Core.Extensions;
using Services.Awaiters;
using Services.Events;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Services
{
    public interface IFileService : IDisposable
    {
        public void UnzipStream(Stream stream, string destination, CancellationToken token);
        public Task<string> SerializeAsync(object o);
        public Task<T> DeserializeAsync<T>(string json);
        public Task<T> LoadJson<T>(string path);
        public bool DeleteFile(string path);
        public bool DeleteDirectory(string path);
        Task<object> LoadJson(string path, Type dtoType);
        public Task<byte[]> LoadBinary(string path);
        public Task<string> LoadString(string path);
        public Task SaveJson(object o, string path);
        public Task SaveBinary(byte[] file, string path);
        public Task SaveBinary(Stream stream, string path);
        public Task SaveString(string file, string path);
        public Task<AudioClip> LoadAudioClip(string path);
        public Task StreamAudio(string path, Action<AudioClip> clipUpdater, float startBuffer = .08f);
        public Task<Texture2D> LoadTexture(string path);
        public Task<AnimationClip> LoadAnimationClip(string path);
        public Task<AssetBundle> LoadAssetBundle(string path);
    }

    public class FileService : IFileService
    {
        private readonly IJsonSerializer _serializer;
        private readonly IDisposable[] _subs;

        public FileService(IJsonSerializer serializer)
        {
            _serializer = serializer;
            _subs = new[]
            {
                EventBus.Subscribe<PersistJsonEvent>(PersistJson),
                EventBus.Subscribe<LoadJsonFileRequestEvent>(LoadJson),
                EventBus.Subscribe<DeleteFileEvent>(@event => { EventBus.Publish(new FileDeletedEvent(@event.Path, DeleteFile(@event.Path)) { }); }),
                EventBus.Subscribe<DeleteDirectoryEvent>(@event => { EventBus.Publish(new DirectoryDeletedEvent(@event.Path, DeleteDirectory(@event.Path))); }),
            };
        }

        private async void PersistJson(PersistJsonEvent jsonEvent)
        {
            await SaveJson(jsonEvent.Content, jsonEvent.Path);
        }

        private async void LoadJson(LoadJsonFileRequestEvent jsonEvent)
        {
            var content = await LoadJsonAsync(jsonEvent.Path, jsonEvent.Type);
            EventBus.Publish(new LoadJsonFileResponseEvent() { Content = content, Path = jsonEvent.Path, IsSecured = jsonEvent.IsSecured });
        }

        public bool DeleteFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;
            try
            {
                var file = new FileInfo(path);
                if (file.Exists)
                    file.Delete();

                return true;
            }
            catch (Exception e)
            {
                LogEvent.AppendException("Failed to delete file!", e);
                return false;
            }
        }

        public bool DeleteDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;
            try
            {
                var dir = new DirectoryInfo(path);
                if (dir.Exists)
                    dir.Delete(true);

                return true;
            }
            catch (Exception e)
            {
                LogEvent.AppendException("Failed to delete directory!", e);
                return false;
            }
        }


        public void UnzipStream(Stream stream, string destination, CancellationToken token)
        {
            using var zip = new ZipArchive(stream);
            foreach (var e in zip.Entries)
                if (!token.IsCancellationRequested)
                {
                    var fileName = Path.Combine(destination, e.FullName);
                    var dir = Path.GetDirectoryName(fileName);
                    if (dir != null && !Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    if (Path.HasExtension(fileName))
                        e.ExtractToFile(fileName, true);
                }
        }

        public async Task<string> SerializeAsync(object o)
        {
            return await Task.Run(() => _serializer.ToJson(o));
        }

        public async Task<T> DeserializeAsync<T>(string json)
        {
            return await Task.Run(() => _serializer.FromJson<T>(json));
        }

        public async Task<T> LoadJson<T>(string path)
        {
            return await LoadJsonAsync<T>(path);
        }

        public async Task<object> LoadJson(string path, Type dtoType)
        {
            return await LoadJsonAsync(path, dtoType);
        }

        public async Task<byte[]> LoadBinary(string path)
        {
            return await LoadBinaryAsync(path);
        }

        public async Task<string> LoadString(string path)
        {
            return await LoadStringAsync(path);
        }

        public async Task SaveJson(object o, string path)
        {
            await SaveJsonAsync(o, path);
        }

        public async Task SaveBinary(byte[] file, string path)
        {
            await SaveBinaryAsync(file, path);
        }

        public async Task SaveBinary(Stream stream, string path)
        {
            await SaveBinaryAsync(stream, path);
        }

        public async Task SaveString(string file, string path)
        {
            await SaveStringAsync(file, path);
        }

        public async Task<AudioClip> LoadAudioClip(string path)
        {
            if (!File.Exists(path))
                return null;
            using var request = UnityWebRequestMultimedia.GetAudioClip(FormatPathForWWWRequest(path), AudioType.OGGVORBIS);
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                LogEvent.AppendError($"Failed to load audio {path}! {request.error}");
                return null;
            }

            return DownloadHandlerAudioClip.GetContent(request);
        }

        public async Task StreamAudio(string path, Action<AudioClip> clipUpdater, float startBuffer = .08f)
        {
            if (!File.Exists(path))
            {
                clipUpdater?.Invoke(null);
                return;
            }

            using var request = UnityWebRequestMultimedia.GetAudioClip(FormatPathForWWWRequest(path), AudioType.OGGVORBIS);
            var downloadHandler = new DownloadHandlerAudioClip(FormatPathForWWWRequest(path), AudioType.OGGVORBIS);
            downloadHandler.streamAudio = true;
            request.downloadHandler = downloadHandler;
            request.SendWebRequest();
            AudioClip previous = null, current = null;
            while (request.downloadProgress < 1f)
            {
                if (request.downloadProgress < startBuffer)
                    return;
                await Task.Delay(10);

                current = downloadHandler.audioClip;
                clipUpdater.Invoke(current);
                if (previous != null)
                    Object.Destroy(previous);
                previous = current;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                LogEvent.AppendError($"Failed to load audio {path}! {request.error}");
                return;
            }

            if (previous != null)
                Object.Destroy(previous);
            clipUpdater.Invoke(current);
        }

        public async Task<Texture2D> LoadTexture(string path)
        {
            if (!File.Exists(path))
                return null;
            using var request = UnityWebRequestTexture.GetTexture(FormatPathForWWWRequest(path));
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                LogEvent.AppendError($"Failed to load texture {path}! {request.error}");
                return null;
            }

            return DownloadHandlerTexture.GetContent(request);
        }

        public async Task<AnimationClip> LoadAnimationClip(string path)
        {
            if (!File.Exists(path))
                return null;
            var assetBundleRequest = await AssetBundle.LoadFromFileAsync(path);
            var assetBundle = assetBundleRequest.assetBundle;

            if (assetBundle == null)
                return null;

            var fileName = Path.GetFileNameWithoutExtension(path);
            var assetLoadRequest = assetBundle.LoadAssetAsync<AnimationClip>(fileName);
            var asset = await assetLoadRequest;
            assetBundle.UnloadAsync(false);
            return asset.asset as AnimationClip;
        }

        public async Task<AssetBundle> LoadAssetBundle(string path)
        {
            if (!File.Exists(path))
                return null;
            var assetBundleRequest = await AssetBundle.LoadFromFileAsync(FormatPathForWWWRequest(path));
            var assetBundle = assetBundleRequest.assetBundle;
            return assetBundle;
        }

        internal async Task SaveStringAsync(string file, string path)
        {
            try
            {
                CreateDirectory(path);
                await File.WriteAllTextAsync(path, file);
            }
            catch (Exception e)
            {
                LogEvent.AppendException($"Failed to save string to path {path}", e);
            }
        }

        internal async Task SaveBinaryAsync(byte[] file, string path)
        {
            try
            {
                CreateDirectory(path);
                await File.WriteAllBytesAsync(path, file);
            }
            catch (Exception e)
            {
                LogEvent.AppendException($"Failed to save file to path {path}", e);
            }
        }

        internal async Task SaveBinaryAsync(Stream stream, string path)
        {
            try
            {
                CreateDirectory(path);
                await using var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                stream.Position = 0;
                fileStream.Position = 0;
                await stream.CopyToAsync(fileStream);
            }
            catch (Exception e)
            {
                LogEvent.AppendException($"Failed to save file to path {path}", e);
            }
        }

        internal async Task SaveJsonAsync(object o, string path)
        {
            try
            {
                CreateDirectory(path);
                await File.WriteAllTextAsync(path, _serializer.ToJson(o));
            }
            catch (Exception e)
            {
                LogEvent.AppendException($"Failed to save json to path {path}", e);
            }
        }

        internal async Task<T> LoadJsonAsync<T>(string path)
        {
            try
            {
                CreateDirectory(path);
                if (!File.Exists(path))
                    return default;
                using var sr = new StreamReader(path);
                var json = await sr.ReadToEndAsync();
                return _serializer.FromJson<T>(json);
            }
            catch (Exception e)
            {
                LogEvent.AppendException($"Failed to load json from path {path}", e);
                return default;
            }
        }

        internal async Task<object> LoadJsonAsync(string path, Type type)
        {
            try
            {
                CreateDirectory(path);
                if (!File.Exists(path))
                    return null;
                using var sr = new StreamReader(path);
                var json = await sr.ReadToEndAsync();
                return _serializer.FromJson(json, type);
            }
            catch (Exception e)
            {
                LogEvent.AppendException($"Failed to load json from path {path}", e);
                return default;
            }
        }

        internal async Task<byte[]> LoadBinaryAsync(string path)
        {
            try
            {
                CreateDirectory(path);
                if (!File.Exists(path))
                    return Array.Empty<byte>();
                return await File.ReadAllBytesAsync(path);
            }
            catch (Exception e)
            {
                LogEvent.AppendException($"Failed to load binary from path {path}", e);
                return Array.Empty<byte>();
            }
        }

        internal async Task<string> LoadStringAsync(string path)
        {
            try
            {
                CreateDirectory(path);
                if (!File.Exists(path))
                    return null;
                return await File.ReadAllTextAsync(path);
            }
            catch (Exception e)
            {
                LogEvent.AppendException($"Failed to load string from path {path}", e);
                return null;
            }
        }

        internal void CreateDirectory(string path)
        {
            DirectoryInfo directory = Path.HasExtension(path) ? new FileInfo(path).Directory : new DirectoryInfo(path);
            if (directory is { Exists: false })
                directory.Create();
        }

        protected string FormatPathForWWWRequest(string path)
        {
            if (Application.isMobilePlatform || Application.platform is RuntimePlatform.OSXEditor or RuntimePlatform.IPhonePlayer)
                return $"file://{path}";
            return path;
        }

        public virtual void Dispose()
        {
            _subs.Dispose();
        }
    }
}