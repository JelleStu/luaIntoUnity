using System;
using HamerSoft.Howl.Sharp.Proxies;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Scripting;

namespace Proxies.GraphicsModule
{
    public class GraphicsModuleProxy : SingletonProxy
    {
        private LuaBridge.Unity.Scripts.LuaBridgeModules.GraphicsModule.GraphicsModule _graphicsModuleTarget;

        [MoonSharpHidden]
        public GraphicsModuleProxy(LuaBridge.Unity.Scripts.LuaBridgeModules.GraphicsModule.GraphicsModule graphicsModuleTarget) : base(graphicsModuleTarget)
        {
            _graphicsModuleTarget = graphicsModuleTarget;
        }

        [Preserve]
        public void SpawnButton(string name, float positionx, float positiony, float width, float height, Action onclick)
        {
            _graphicsModuleTarget.Spawnbutton(name, new Vector2(positionx, positiony), width, height, onclick);
        }

        [Preserve]
        public void MoveElement(string name, float positionx, float positiony)
        {
            _graphicsModuleTarget.MoveElement(name, new Vector2(positionx, positiony));
        }

        public void MoveButtonToLocationWithDoTween(string name, float endPositionX, float endPositionY, float time, Action callback)
        {
            _graphicsModuleTarget.MoveButtonWithDoTween(name, endPositionX, endPositionY, time, callback);
        }

        public void Update()
        {
            _graphicsModuleTarget.Update();
        }

        protected override string LuaName => "GraphicsModuleProxy";
    }
}