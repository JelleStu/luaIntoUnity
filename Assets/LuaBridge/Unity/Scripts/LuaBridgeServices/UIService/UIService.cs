using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuaBridge.Unity.Scripts.LuaBridgeServices.UIService
{
    public interface IUIService : IDisposable
    {
        public void SpawnButton(string key, Vector2 position, float width, float height, Action onclick);
        public void MoveElement(string key, Vector2 newPosition);
        public List<string> GetAllKeys();
        public List<T> GetAllElementsFromType<T>(T type);
        public RectTransform GetElementByKey(string key);
        public void MoveElementWithDoTween(string key, Vector2 endposition, float time);
    }
    
    public class UGuiService : IUIService
    {

        private Canvas _canvas;
        
        public UGuiService(Canvas canvas)
        {
            _canvas= canvas;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SpawnButton(string key, Vector2 position, float width, float height, Action onclick)
        {
            throw new NotImplementedException();
        }

        public void MoveElement(string key, Vector2 newPosition)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllKeys()
        {
            throw new NotImplementedException();
        }

        public List<T> GetAllElementsFromType<T>(T type)
        {
            throw new NotImplementedException();
        }

        public RectTransform GetElementByKey(string key)
        {
            throw new NotImplementedException();
        }

        public void MoveElementWithDoTween(string key, Vector2 endposition, float time)
        {
            throw new NotImplementedException();
        }
    }
}