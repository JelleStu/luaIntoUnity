using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface;
using Services;
using Services.Prefab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Services
{
    public class UGuiCanvasService : IUIService
    {
        
        public RootView Root { get; private set; }
        private readonly IPrefabService _prefabService;
        private IFileService _fileService;
        private readonly Canvas _canvas;
        private Button buttonPrefab;
        private TextMeshProUGUI textLabelPrefab;
        private  JeltieboyImage imageprefab;
        private Dictionary<string, Component> _elements;
        private Texture2D kajkft;

        public UGuiCanvasService(IPrefabService prefabService, Canvas canvas)
        {
            _prefabService = prefabService;
            _canvas = canvas;
            _elements = new Dictionary<string, Component>();
            Root = _canvas.GetComponentInChildren<RootView>();
        }
        

        public void Boot()
        {
            buttonPrefab = _prefabService.GetPrefab<Button>();
            textLabelPrefab = _prefabService.GetPrefab<TextMeshProUGUI>();
            imageprefab = _prefabService.GetPrefab<JeltieboyImage>();
        }

        #region Create methods

        public void CreateButton(string key, Rect rect, string text, Action onclick)
        {
            var button = Object.Instantiate(buttonPrefab, _canvas.transform);
            button.name = key;
            if (button.transform is RectTransform rectTransform)
            {
                rectTransform.pivot = new Vector2(.5f, .5f);
                rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
                rectTransform.anchoredPosition = rect.position;
            }
            button.onClick.AddListener(() => onclick?.Invoke());
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;
            _elements.Add(key, button);
        }

        public void CreateTextLabel(string key, Rect rect, string text)
        {
            var textLabel = Object.Instantiate(textLabelPrefab, _canvas.transform);
            textLabel.name = key;
            if (textLabel.transform is RectTransform rectTransform)
            {
                rectTransform.pivot = new Vector2(.5f, .5f);
                rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
                rectTransform.anchoredPosition = rect.position;
            }

            textLabel.text = text;
            _elements.Add(key, textLabel);        
        }

        public void CreateImage(string key, Rect rect, string sourceImage)
        {
            var image = Object.Instantiate(imageprefab, _canvas.transform);
            if (image.transform is RectTransform rectTransform)
            {
                rectTransform.pivot = new Vector2(.5f, .5f);
                rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
                rectTransform.anchoredPosition = rectTransform.position;
            }

            if (sourceImage == null)
                return;
            var test = LoadImage(sourceImage).Result;
            image.Image.sprite = Sprite.Create(test, rect, new Vector2(.5f, .5f));
            _elements.Add(key, image);
        }

        #endregion


        #region Edit methods
       

        public void SetButtonText(string key, string newtext)
        {
            Button button = (Button )GetElementByKey(key);
            if (button == null)
                Debug.LogError("Cant find button with key {key}");
            
            button.GetComponentInChildren<TextMeshProUGUI>().text = newtext;
        }


        public void SetTextLabelText(string elementKey, string newText)
        {
            var textLabel = (TextMeshProUGUI) GetElementByKey(elementKey);
            textLabel.text = newText;
        }

        #endregion

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

        public List<T> GetAllElementsFromType<T>() where T : Component
        {
            List<T> list = new List<T>();
            foreach (var kvp in _elements)
            {
                if (kvp.Value is T value)
                {
                    list.Add(value);
                }
            }

            return list;
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

        public  Component GetElementByKey(string key)
        {
            return _elements.TryGetValue(key, out var element) ? element : null;
        }

        #region helpers
        private async Task<Texture2D> LoadImage(string sourceImage)
        {
            return await _fileService.LoadTexture(sourceImage); 
        }
        

        #endregion

        public void Dispose()
        {
            _elements.Clear();
        }

        public void SetFileService(IFileService getService)
        {
            _fileService = getService;
        }
    }
}