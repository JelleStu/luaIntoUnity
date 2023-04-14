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

        #region Spawn elements methods

        public void CreateButton(string name, Vector2 position, float width, float height,string text, Action onclick)
        {
            UIService.CreateButton(name, position, width, height, text,onclick);
        }
        
        public void CreateTextLabel(string name, Vector2 position, float width, float height, string text)
        {
            UIService.CreateTextLabel(name, position, width, height, text);
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
        
    }
}