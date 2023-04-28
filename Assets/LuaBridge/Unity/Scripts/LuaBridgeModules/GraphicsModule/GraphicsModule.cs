using System;
using System.Threading.Tasks;
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

        public void CreateButton(string elementKey, Rect rect, string text, Action onclick)
        {
            UIService.CreateButton(elementKey, rect, text, onclick);
        }
        
        public void CreateTextLabel(string elementKey, Rect rect, string text)
        {
            UIService.CreateTextLabel(elementKey, rect, text);
        }
        
        public async Task CreateImage(string elementKey, Rect rect, string sourceImageName)
        {
            await UIService.CreateImage(elementKey, rect, sourceImageName);
        }

        public void DeleteElement(string elementKey)
        {
            UIService.DeleteElement(elementKey);
        }

        #endregion


        #region Edit elements methods

        public void SetTextLabelText(string elementKey, string newText)
        {
            UIService.SetTextLabelText(elementKey, newText);
        }
        
        public async Task ChangeImage(string elementKey, string pathToOtherImage)
        {
            await UIService.ChangeImage(elementKey, pathToOtherImage);
        }

        #endregion
        public void MoveElement(string elementKey, Vector2 newPosition)
        {
            UIService.MoveElement(elementKey, newPosition);
        }
        
        public void Update()
        {
            //UIService.Update()
        }

        public void SetButtonText(string elementKey, string newtext)
        {
            UIService.SetButtonText(elementKey, newtext);
        }

        public Component GetElementByKey(string elementKey)
        {
           return UIService.GetElementByKey(elementKey);
        }



    }
}