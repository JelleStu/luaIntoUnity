using System;
using LuaBridge.Modules.Audio;
using MoonSharp.Interpreter;
using UnityEngine.Scripting;

namespace LuaBridge.Proxies.Audio
{
    public class AudioModuleProxy
    { 
        private AudioModule _target;

        [MoonSharpHidden]
        public AudioModuleProxy(AudioModule audioModuleTarget)
        {
            this._target = audioModuleTarget;
        }
        [Preserve]
        public void Play(Action<string> callback)
        {
            _target.Play(callback);
        }
    }
}