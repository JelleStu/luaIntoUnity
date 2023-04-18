using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public RootView Root { get; private set; }
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
            Root = _canvas.GetComponentInChildren<RootView>();
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
        
        public void CreateButton(string key, Vector2 position, float width, float height, string text, Action onclick)
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
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;
            _elements.Add(key, button);
        }

        public void SetButtonText(string key, string newtext)
        {
            Button button = (Button )GetElementByKey(key);
            if (button == null)
                Debug.LogError("Cant find button with key {key}");
            
            button.GetComponentInChildren<TextMeshProUGUI>().text = newtext;

            
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

        public void SetTextLabelText(string elementKey, string newText)
        {
            var textLabel = (TextMeshProUGUI) GetElementByKey(elementKey);
            textLabel.text = newText;
        }

        public void MoveElement(string key, Vector2 newPosition)
        {
            RectTransform element = GetRectTransformByKey(key);
            if (element != null)
                element.anchoredPosition = newPosition;
            else
                Debug.LogError("Element is not registered.");
        }

        public List<string> GetAllKeys()
        {
            return _elements.Keys.ToList();
        }


        public List<T> GetAllElementsFromType<T>(T type)
        {
            throw new NotImplementedException();
        }

        public RectTransform GetRectTransformByKey(string key)
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

        public void DeleteElement(string key)
        {
            var element = GetElementByKey(key);
            if (element == null)
                Debug.LogError($"Could not destroy element with key {key}");
            Object.Destroy(element.gameObject);
            _elements.Remove(key);
        }

        private MonoBehaviour GetElementByKey(string key)
        {
            return _elements.TryGetValue(key, out var element) ? element : null;
        }
            
        
        public void Dispose()
        {
            _elements.Clear();
        }
    }
}