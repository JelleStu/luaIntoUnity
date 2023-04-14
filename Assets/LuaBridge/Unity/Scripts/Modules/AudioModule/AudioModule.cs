using System;
using System.Threading.Tasks;
using LuaBridge.Core.Services.Abstract;
using Services;
using UnityEngine;

namespace LuaBridge.Modules.Audio
{
    public class AudioModule : IAsyncBootService
    {
        private AudioSource _audioSource;
        private IFileService _fileService;


        public AudioModule()
        {
            _audioSource = new GameObject().AddComponent<AudioSource>();
        }

        private async Task LoadFile()
        {
            var audioclip = await _fileService.LoadAudioClip($"{Application.dataPath}/Resources/Audio/viezegothic.ogg");
             _audioSource.clip = audioclip;
        }
        
        public void Play(Action<string> callback)
        {
            if (_audioSource.isPlaying)
                _audioSource.Stop();
            _audioSource.Play();
            callback?.Invoke("sound played complete");

        }

        public void SetAudioService(IFileService getService)
        {
            _fileService = getService;
        }

        public void Dispose()
        {
            
        }

        public async Task Boot()
        {
            await LoadFile();
        }
    }
}