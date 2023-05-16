using System;
using HamerSoft.Howl.Sharp.Proxies;
using LuaBridge.Unity.Scripts.LuaBridgeModules.AudioModule;
using MoonSharp.Interpreter;
using UnityEngine.Scripting;

namespace LuaBridge.Unity.Scripts.LuaBridgesProxies.AudioService
{
    public class AudioServiceProxy : SingletonProxy, IDisposable
    {
        protected override string LuaName => "AudioServiceProxy";
        private AudioModule _audioModuleTarget;
        
        [MoonSharpHidden]
        public AudioServiceProxy(AudioModule audioModuleTarget) : base(audioModuleTarget)
        {
            _audioModuleTarget = audioModuleTarget;
        }
        [Preserve]
        public async void PlayAudioClip(string audioClip)
        {
            await _audioModuleTarget.PlayAudio(audioClip);
        }
        
        [Preserve]
        public void SetVolume(int volume)
        {
            _audioModuleTarget.SetVolume(volume);
        }
        
        [Preserve]
        public void StopPlayingAudio()
        {
            _audioModuleTarget.StopPlayingAudio();
        }
        [Preserve]
        public void Dispose()
        {
            _audioModuleTarget = null;
        }
    }
}