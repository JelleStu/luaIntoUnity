using System;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface;
using UnityEngine;

namespace LuaBridge.Unity.Scripts.LuaBridgeModules.GraphicsModule
{
    public class GraphicsModule
    {
        private readonly IUIService UIService;

        public GraphicsModule(IUIService uiService)
        {
            this.UIService = uiService;
        }

        public void Spawnbutton(string name, Vector2 position, float width, float height, Action onclick)
        {
            UIService.SpawnButton(name, position, width, height, onclick);
        }

        public void MoveElement(string name, Vector2 newPosition)
        {
            UIService.MoveElement(name, newPosition);
        }

        public void StartCoroutine()
        {
        }

        public void Update()
        {
            //UIService.Update()
        }

        public void MoveButtonWithDoTween(string name, float endPositionX, float endPositionY, float time, Action callback)
        {
            UIService.MoveElementWithDoTweenCallback(name, new Vector2(endPositionX, endPositionY), time, callback);
        }
    }
}