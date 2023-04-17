using System;
using System.Collections.Generic;
using LuaBridge.Core.Services.Abstract;
using UnityEngine;

namespace LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface
{
    public interface IUIService : IBootService, IDisposable
    {
        public RootView Root { get; }

        public void CreateButton(string key, Vector2 position, float width, float height, string text, Action onclick);
        public void SetButtonText(string key, string newtext);
        public void CreateTextLabel(string key, Vector2 position, float width, float height, string text);
        public void SetTextLabelText(string elementKey, string newText);
        public void MoveElement(string key, Vector2 newPosition);
        public List<string> GetAllKeys();
        public List<T> GetAllElementsFromType<T>(T type);
        public RectTransform GetRectTransformByKey(string key);
    }
}