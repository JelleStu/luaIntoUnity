using System;
using System.Collections.Generic;
using HamerSoft.Howl.Core;
using LuaBridge.Core.Services.Abstract;
using UnityEngine;

namespace LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface
{
    public interface IUIService : IBootService, IDisposable
    {
        public void SpawnButton(string key, Vector2 position, float width, float height, Action onclick);
        public void MoveElement(string key, Vector2 newPosition);
        public List<string> GetAllKeys();
        public List<T> GetAllElementsFromType<T>(T type);
        public RectTransform GetElementByKey(string key);
        public void MoveElementWithDoTween(string key, Vector2 endposition, float time);
        public void MoveElementWithDoTweenCallback(string key, Vector2 endposition, float time, Action callback);
    }
}