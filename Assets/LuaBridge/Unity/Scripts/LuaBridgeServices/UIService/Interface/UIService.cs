using System;
using System.Collections.Generic;
using LuaBridge.Core.Services.Abstract;
using Services;
using UnityEngine;

namespace LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface
{
    public interface IUIService : IBootService, IDisposable
    {
        public RootView Root { get; }

        public void CreateButton(string key, Rect rect, string text, Action onclick);
        public void SetButtonText(string key, string newtext);
        public void CreateTextLabel(string key, Rect rect, string text);
        public void SetTextLabelText(string elementKey, string newText);
        public void MoveElement(string key, Vector2 newPosition);
        public List<string> GetAllKeys();
        public List<T> GetAllElementsFromType<T>() where T : Component;
        public RectTransform GetRectTransformByKey(string key);
        void DeleteElement(string key);
        Component GetElementByKey(string key);
        void CreateImage(string key, Rect rect, string sourceImage);
        void SetFileService(IFileService getService);
    }
}