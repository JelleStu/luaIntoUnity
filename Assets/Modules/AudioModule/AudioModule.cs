using System;
using System.Threading;
using DefaultNamespace;
using Services;
using UnityEngine;

namespace Lunacy.Modules.Audio
{
    public class AudioModule
    {
        private AudioSource _audioSource;
        private FileService _fileService;


        public AudioModule()
        {
            _audioSource = new GameObject().AddComponent<AudioSource>();
            _fileService = new FileService(new Serializer());
            LoadFile();
        }

        private async void LoadFile()
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
    }
}