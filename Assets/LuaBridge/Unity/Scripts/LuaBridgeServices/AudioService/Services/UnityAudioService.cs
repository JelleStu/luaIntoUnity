using System.IO;
using System.Threading.Tasks;
using LuaBridge.Unity.Scripts.LuaBridgeServices.AudioService.Interface;
using Services;
using UnityEngine;

namespace LuaBridge.Unity.Scripts.LuaBridgeServices.AudioService.Services
{
    public class UnityAudioService : IAudioService
    {
        private string sandboxRoot;
        private AudioSource _audioSource;
        private IFileService _fileService;
        
        public UnityAudioService(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }
        public void SetVolume(int volume)
        {
            _audioSource.volume = volume;
        }

        public async Task PlayAudioClip(string audioClip)
        {
            if (_audioSource.isPlaying)
                StopPlayingAudio();
            
            AudioClip clip =  await LoadFile(audioClip);
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void StopPlayingAudio()
        {
            _audioSource.Stop();
        }

        private async Task<AudioClip> LoadFile(string audioClip)
        {
            string path = Path.Combine(sandboxRoot,"Assets","Sound", audioClip);
            if (!File.Exists(path))
            {
                Debug.LogError($"sound file doesnt exist {path}");
            }
            return await _fileService.LoadAudioClip(path);
        }
        
        public void SetFileService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public void SetSandboxRoot(string path)
        {
            sandboxRoot = path;
        }
        

    }
}