using System;
using HamerSoft.Howl.Sharp.Proxies;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Scripting;

namespace Proxies.GraphicsModule
{
    public class GraphicsModuleProxy : SingletonProxy
    {
        protected override string LuaName => "GraphicsModuleProxy";

        private LuaBridge.Unity.Scripts.LuaBridgeModules.GraphicsModule.GraphicsModule _graphicsModuleTarget;

        [MoonSharpHidden]
        public GraphicsModuleProxy(LuaBridge.Unity.Scripts.LuaBridgeModules.GraphicsModule.GraphicsModule graphicsModuleTarget) : base(graphicsModuleTarget)
        {
            _graphicsModuleTarget = graphicsModuleTarget;
        }

        [Preserve]
        public void CreateButton(string name, float positionx, float positiony, float width, float height, Action onclick)
        {
            _graphicsModuleTarget.CreateButton(name, new Vector2(positionx, positiony), width, height, onclick);
        }
        
        [Preserve]
        public void CreateTextLabel(string name, float positionx, float positiony, float width, float height, string text)
        {
            _graphicsModuleTarget.CreateTextLabel(name, new Vector2(positionx, positiony), width, height, text);
        }

        [Preserve]
        public void MoveElement(string name, float positionx, float positiony)
        {
            _graphicsModuleTarget.MoveElement(name, new Vector2(positionx, positiony));
        }
    }
}