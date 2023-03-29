using System;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Scripting;

namespace Proxies.GraphicsModule
{
    public class GraphicsModuleProxy
    {
        private Modules.Graphics.GraphicsModule _graphicsModuleTarget;

        [MoonSharpHidden]
        public GraphicsModuleProxy(Modules.Graphics.GraphicsModule graphicsModuleTarget)
        {
            _graphicsModuleTarget = graphicsModuleTarget;
        }

        [Preserve]
        public void SpawnButton(string name, float positionx, float positiony, float width, float height, Action onclick)
        {
            _graphicsModuleTarget.Spawnbutton( name, new Vector2(positionx, positiony),  width,  height,  onclick);
        }
        
        [Preserve]
        public void MoveElement(string name, float positionx, float positiony)
        {
            _graphicsModuleTarget.MoveElement(name, new Vector2(positionx, positiony));
        }

        public void StartCoroutine()
        {
            _graphicsModuleTarget.StartCoroutine();
        }
    }
}