using System;
using Luncay.Core;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Modules.Graphics
{
    public class GraphicsModule
    {
        public GraphicsModule()
        {
        }

        public void Spawnbutton(string name, Vector2 position, float width, float height, Action onclick)
        {
            CanvasManager.instance.SpawnButton(name, position, width, height, onclick);
        }

        public void MoveElement(string name, Vector2 newPosition)
        {
            CanvasManager.instance.MoveElement(name, newPosition);
        }

        public void StartCoroutine()
        {
            FakeCoreModule.fakeCoreModule.StartTheMovement();
        }

        public void Update()
        {
            CanvasManager.instance.DebugLog();
        }

        public void MoveButtonWithDoTween(string name, float endPositionX, float endPositionY, float time, DynValue callback)
        {
            CanvasManager.instance.MoveWithDotween(name, endPositionX, endPositionY, time, callback);
        }
    }
}