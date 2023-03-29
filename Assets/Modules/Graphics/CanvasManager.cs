using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Modules.Graphics;
using Unity.VisualScripting;
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
            if (_elements.TryGetValue(key, out var element))
            {
                if (element.transform is RectTransform rectTransform)
                {
                    rectTransform.anchoredPosition = newPosition;
                }
            }
        }
    }
}