using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LuaBridge.Core.Services.Abstract;
using Services;
using UnityEngine;

namespace LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface
{
    public interface IUIService : IBootService, IDisposable
    {
        public RootView Root { get; }

        public void CreateButton(string elementKey, Rect rect, string text, Action onclick);
        public void SetButtonText(string elementKey, string newtext);
        public void CreateTextLabel(string elementKey, Rect rect, string text);
        public void SetTextLabelText(string elementKey, string newText);
        Task CreateImage(string elementKey, Rect rect, string sourceImageName);
        Task ChangeImage(string elementKey, string sourceToNewImageName);
        public void MoveElement(string elementKey, Vector2 newPosition);
        public List<string> GetAllKeys();
        public List<T> GetAllElementsFromType<T>() where T : Component;
        public RectTransform GetRectTransformByKey(string elementKey);
        void DeleteElement(string elementKey);
        Component GetElementByKey(string elementKey);
        void SetFileService(IFileService getService);
        void SetSandBoxRootDirectory(string path);
    }
}