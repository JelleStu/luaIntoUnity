using System;
using Luncay.Core;
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
        
    }
}