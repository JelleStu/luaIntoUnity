using System;
using System.Collections.Generic;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface;
using Services.Prefab;
using TMPro;
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
        private Button buttonPrefab;
        private TextMeshProUGUI textLabelPrefab;
        private Dictionary<string, MonoBehaviour> _elements;

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
            buttonPrefab = _prefabService.GetPrefab<Button>();
            textLabelPrefab = _prefabService.GetPrefab<TextMeshProUGUI>();
        }
        
        public void CreateButton(string key, Vector2 position, float width, float height, Action onclick)
        {
            var button = Object.Instantiate(buttonPrefab, _canvas.transform);
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

        public void CreateTextLabel(string key, Vector2 position, float width, float height, string text)
        {
            var textLabel = Object.Instantiate(textLabelPrefab, _canvas.transform);
            textLabel.name = key;
            if (textLabel.transform is RectTransform rt)
            {
                rt.pivot = new Vector2(.5f, .5f);
                rt.sizeDelta = new Vector2(width, height);
                rt.anchoredPosition = position;
            }

            textLabel.text = text;
            _elements.Add(key, textLabel);        }

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
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}