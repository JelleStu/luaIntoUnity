using System;
using System.Collections.Generic;
using DG.Tweening;
using LuaBridge.Core.Services.Abstract;
using Luncay.Core;
using UnityEngine.UI;
using MoonSharp.Interpreter;
using Services.Prefab;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules
{
    public class CanvasManager : IBootService
    {
        public static CanvasManager instance { get; private set; }
        private Button ButtonPrefab;
        private Dictionary<string, MonoBehaviour> _elements;
        private readonly IPrefabService _prefabService;
        private readonly Canvas _canvas;
        private readonly RootView _root;

        public CanvasManager(IPrefabService prefabService, Canvas canvas)
        {
            _prefabService = prefabService;
            _canvas = canvas;
            _root = _canvas.GetComponentInChildren<RootView>();
            instance = this;
            _elements = new Dictionary<string, MonoBehaviour>();
            Debug.Log("awake");
            
            /*Task.Run(() =>
            {
                var script = new Script();
                script.Options.DebugPrint = Debug.Log;
                script.DoString("print(\"fuck\")");
            });*/
        }
        
        public void Boot()
        {
            ButtonPrefab = _prefabService.GetPrefab<Button>();
        }
        
        

        public void SpawnButton(string _buttonName, Vector2 position, float width, float height, Action onclick)
        {
            var button = Object.Instantiate(ButtonPrefab, _canvas.transform);
            button.name = _buttonName;
            if (button.transform is RectTransform rt)
            {
                rt.pivot = new Vector2(.5f, .5f);
                rt.sizeDelta = new Vector2(width, height);
                rt.anchoredPosition = position;
            }
            button.onClick.AddListener(() => onclick.Invoke());
            _elements.Add(_buttonName, button);
        }

        public void MoveElement(string key, Vector2 newPosition)
        {
            RectTransform element = GetElementByName(key);
            if (element != null)
                element.anchoredPosition = newPosition;
            else
                Debug.LogError("Element is not registered.");
        }

        public List<string> GetAllKeys()
        {
            List<string> keys = new List<string>();
            foreach (var keyValuePair in _elements)
            {
                keys.Add(keyValuePair.Key);
            }
            return keys;
        }

        public void DebugLog()
        {   
            Debug.Log("Test");
        }

        public void MoveWithDotween(string name, float endPositionX, float endPositionY, float time, DynValue callback)
        {
            GetElementByName(name).DOAnchorPos(new Vector3(endPositionX, endPositionY),5).OnComplete(() =>
            {
                // Call function
                if (callback != null)
                    FakeCoreModule.fakeCoreModule.CallFunction(callback);
            });
        }

        private RectTransform GetElementByName(string name)
        {
            if (_elements.TryGetValue(name, out var element))
            {
                if (element.transform is RectTransform rectTransform)
                {
                    return rectTransform;
                }
            }
            return null;
        }

    }
}