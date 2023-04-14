﻿using System;
using HamerSoft.Howl.Sharp.Proxies;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Scripting;

namespace LuaBridge.Unity.Scripts.LuabridgeProxies.GraphicsService
{
    public class GraphicsServiceProxy : SingletonProxy
    {
        protected override string LuaName => "GraphicsServiceProxy";

        private LuaBridgeModules.GraphicsModule.GraphicsModule _graphicsModuleTarget;

        [MoonSharpHidden]
        public GraphicsServiceProxy(LuaBridgeModules.GraphicsModule.GraphicsModule graphicsModuleTarget) : base(graphicsModuleTarget)
        {
            _graphicsModuleTarget = graphicsModuleTarget;
        }

        [Preserve]
        public void CreateButton(string name, float positionx, float positiony, float width, float height,string text, Action onclick)
        {
            _graphicsModuleTarget.CreateButton(name, new Vector2(positionx, positiony), width, height, text, onclick);
        }
        
        [Preserve]
        public void SetTextLabelText(string key, string newtext)
        {
            _graphicsModuleTarget.SetTextLabelText(key, newtext);
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