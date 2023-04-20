using System;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface;
using UnityEngine;

namespace LuaBridge.Unity.Scripts.LuaBridgeModules.GraphicsModule
{
    public class GraphicsModule
    {
        private readonly IUIService UIService;

        public GraphicsModule(IUIService uiService)
        {
            UIService = uiService;
        }

        #region Create elements methods

        public void CreateButton(string name, Rect rect, string text, Action onclick)
        {
            UIService.CreateButton(name, rect, text, onclick);
        }
        
        public void CreateTextLabel(string name, Rect rect, string text)
        {
            UIService.CreateTextLabel(name, rect, text);
        }
        
        public void CreateImage(string imageName, Rect rect, string sourceImage)
        {
            UIService.CreateImage(imageName, rect, sourceImage);
        }

        public void DeleteElement(string key)
        {
            UIService.DeleteElement(key);
        }

        #endregion


        #region Edit elements methods

        public void SetTextLabelText(string elementKey, string newText)
        {
            UIService.SetTextLabelText(elementKey, newText);
        }

        #endregion
        public void MoveElement(string name, Vector2 newPosition)
        {
            UIService.MoveElement(name, newPosition);
        }
        
        public void Update()
        {
            //UIService.Update()
        }

        public void SetButtonText(string key, string newtext)
        {
            UIService.SetButtonText(key, newtext);
        }

        public Component GetElementByKey(string key)
        {
           return UIService.GetElementByKey(key);
        }


    }
}