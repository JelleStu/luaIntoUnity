using System;
using System.Collections.Generic;
using DG.Tweening;
using HamerSoft.Howl.Core;
using LuaBridge.Core.Services.Abstract;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface;
using LuaBridge.Unity.Scripts.LuaBridgesGames.Managers;
using MoonSharp.Interpreter;
using Services.Prefab;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Services
{
    public class UGuiCanvasService : IUIService
    {
        private readonly IPrefabService _prefabService;
        private readonly Canvas _canvas;
        private Button ButtonPrefab;
        private Dictionary<string, MonoBehaviour> _elements;
        private Random rand;

        public UGuiCanvasService(IPrefabService prefabService, Canvas canvas)
        {
            _prefabService = prefabService;
            _canvas = canvas;
            _elements = new Dictionary<string, MonoBehaviour>();
            /*Task.Run(() =>
            {
                var script = new Script();
                script.Options.DebugPrint = Debug.Log;
                script.DoString("print(\"fuck\")");
            });*/
        }
        
        public void Boot()
        {
            rand = new Random();
            ButtonPrefab = _prefabService.GetPrefab<Button>();
        }
        
        public void SpawnButton(string key, Vector2 position, float width, float height, Action onclick)
        {
            var button = Object.Instantiate(ButtonPrefab, _canvas.transform);
            button.name = key;
            if (button.transform is RectTransform rt)
            {
                rt.pivot = new Vector2(.5f, .5f);
                rt.sizeDelta = new Vector2(width, height);
                rt.anchoredPosition = position;
            }
            button.onClick.AddListener(() => onclick?.Invoke());
            _elements.Add(key, button);
        }

        public void MoveElement(string key, Vector2 newPosition)
        {
            RectTransform element = GetElementByKey(key);
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

        public void MoveWithDotween(string key, float endPositionX, float endPositionY, float time, Action callback)
        {

        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<T> GetAllElementsFromType<T>(T type)
        {
            throw new NotImplementedException();
        }

        public RectTransform GetElementByKey(string key)
        {
            if (_elements.TryGetValue(key, out var element))
            {
                if (element.transform is RectTransform rectTransform)
                {
                    return rectTransform;
                }
            }
            return null;        
        }

        public void MoveElementWithDoTween(string key, Vector2 endposition, float time)
        {
            throw new NotImplementedException();
        }

        public void MoveElementWithDoTweenCallback(string key, Vector2 endposition, float time, Action callback)
        {
            GetElementByKey(key).DOAnchorPos(new Vector3(endposition.x, endposition.y),time).OnComplete(() =>
            {
                callback?.Invoke();
            }).SetEase(Ease.InOutCubic);
        }
    }
}