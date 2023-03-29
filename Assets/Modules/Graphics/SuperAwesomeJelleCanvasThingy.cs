using System;
using UnityEngine;

namespace Modules.Graphics
{
    public class SuperAwesomeJelleCanvasThingy : IUIElement
    {
        public Canvas Canvas;

        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }
    }
}