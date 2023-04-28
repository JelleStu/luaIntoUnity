using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LuaBridge.Core.Extensions;
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
        private JeltieboyImage imageprefab;
        private Dictionary<string, Component> _elements;
        private string _sandBoxRootDirectory;

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

        public async Task CreateImage(string key, Rect rect,string sourceImageName)
        {
            var image = Object.Instantiate(imageprefab, _canvas.transform);
            if (image.transform is RectTransform rectTransform)
            {
                rectTransform.pivot = new Vector2(.5f, .5f);
                rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
                rectTransform.anchoredPosition = rect.position;
            }

            string pathToImage = $"{Path.Combine(_sandBoxRootDirectory, "Assets", "Images", sourceImageName)}";
            if (string.IsNullOrEmpty(sourceImageName) || !File.Exists(pathToImage))
            {
                Debug.LogError($"Can not find image with path {pathToImage}");
                return;
            }

            Texture2D texture = await LoadImage(pathToImage);
            if (texture == null)
            {
                Debug.LogError("Texture did not load!");
                return;
            }
            image.Image.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(.5f, .5f));
            _elements.Add(key, image);
        }

        #endregion


        #region Edit methods

        public void SetButtonText(string key, string newtext)
        {
            if(!_elements.TryGetValueAs(key, out Button button));
            if (button == null)
            {
                Debug.LogError($"Can not find button with key {key}");
                return;
            }

            button.GetComponentInChildren<TextMeshProUGUI>().text = newtext;
        }
        
        public void SetTextLabelText(string elementKey, string newText)
        {
            if (!_elements.TryGetValueAs(elementKey, out TextMeshProUGUI textLabel))
            {
                Debug.LogError($"Can not find textlabel with key {elementKey}");
                return;
            }
            textLabel.text = newText;
        }

        public async Task ChangeImage(string elementKey, string sourceNewImageName)
        {
            if (!_elements.TryGetValueAs(elementKey, out JeltieboyImage image))
            {
                Debug.LogError($"Could not find image with key {elementKey}");
                return;
            }

            string pathToNewImage = $"{Path.Combine(_sandBoxRootDirectory, "Assets", "Images", sourceNewImageName)}";

            if (string.IsNullOrEmpty(sourceNewImageName) || !File.Exists(pathToNewImage))
            {
                Debug.LogError($"Can not find image with path {pathToNewImage}");
                return;
            }

            Texture2D newTexture = await LoadImage(pathToNewImage);
            if (newTexture == null)
                Debug.LogError($"Can't convert the newtexture!"); 
            image.Image.sprite = Sprite.Create(newTexture, new Rect(0.0f, 0.0f, newTexture.width, newTexture.height),new Vector2(.5f, .5f));
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

        public Component GetElementByKey(string key)
        {
            if (_elements.TryGetValue(key, out var element)) 
                return element;
            Debug.LogError($"Cannot find element with key {key}");
            return null;

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

        public void SetSandBoxRootDirectory(string path)
        {
            _sandBoxRootDirectory = path;
        }
    }
}