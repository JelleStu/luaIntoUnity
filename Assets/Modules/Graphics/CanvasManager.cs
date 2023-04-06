using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Luncay.Core;
using UnityEngine.UI;
using Modules.Graphics;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Modules
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager instance { get; private set; }
        [SerializeField] private SuperAwesomeJelleCanvasThingy _canvas;
        [SerializeField] private Button ButtonPrefab;
        private Dictionary<string, MonoBehaviour> _elements;

        private void Awake()
        {
            instance = this;
            _elements = new Dictionary<string, MonoBehaviour>();
            /*Task.Run(() =>
            {
                var script = new Script();
                script.Options.DebugPrint = Debug.Log;
                script.DoString("print(\"fuck\")");
            });*/
        }

        public void SpawnButton(string name, Vector2 position, float width, float height, Action onclick)
        {
            var button = Instantiate(ButtonPrefab, _canvas.Canvas.transform);
            button.name = name;
            if (button.transform is RectTransform rt)
            {
                rt.pivot = new Vector2(.5f, .5f);
                rt.sizeDelta = new Vector2(width, height);
                rt.anchoredPosition = position;
            }
            button.onClick.AddListener(() => onclick.Invoke());
            _elements.Add(name, button);
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