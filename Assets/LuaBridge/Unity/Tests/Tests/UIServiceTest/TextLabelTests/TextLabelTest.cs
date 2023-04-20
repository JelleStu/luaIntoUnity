using System.Collections;
using System.Threading.Tasks;
using LuaBridge.Core.Configuration;
using LuaBridge.Unity.Scripts.LuaBridgeModules.GraphicsModule;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Services;
using LuaBridge.Unity.Tests.Tests.UIServiceTest.Abstract;
using NUnit.Framework;
using Services.Prefab;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace LuaBridge.Unity.Tests.Tests.UIServiceTest.TextLabelTests
{
    public class TextLabelTest : LuaBridgeTest
    {
        protected Canvas Canvas;
        private GraphicsModule graphicsModule;
        private IUIService uiService;

        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            Canvas = new GameObject("Canvas").AddComponent<Canvas>();

            yield return base.SetUp();
            uiService = AppContainer.GetService<IUIService>();
            graphicsModule = new GraphicsModule(uiService);
        }

        [UnityTearDown]
        public override IEnumerator TearDown()
        {
            AppContainer.Dispose();
            AppContainer = null;
            yield return null;
        }

        protected override async Task<AppContainer> CreateContainer()
        {
            var container = new AppConfiguration()
                .AddSingleton<IUIService, UGuiCanvasService>(Canvas)
                .AddSingleton<IPrefabService, PrefabService>("PrefabRegistry")
                .Build();

            await Task.WhenAll(
                container.GetService<IPrefabService>().Boot());
            container.GetService<IUIService>().Boot();

            return container;
        }
        
        
        [Test]
        public void TestCreateTextLabel()
        {
            string key = "testTextLabel";
            graphicsModule.CreateTextLabel(key, new Rect(1000, 500, 250, 250), "test");
            var textLabelObject = Canvas.GetComponentInChildren<TextMeshProUGUI>();
            graphicsModule.GetElementByKey(key);
            Assert.AreEqual(1, uiService.GetAllKeys().Count);
            Assert.NotNull(textLabelObject);

        }
        
        [Test]
        public void TestGetTextLabelWithKey()
        {
            string textLabelKey = "textLabelKey";
            graphicsModule.CreateButton(textLabelKey, new Rect(1000, 500, 250, 250), "test", () => Debug.Log("clicked"));
            var buttonMonoBehaviour = uiService.GetElementByKey(textLabelKey);
            Assert.AreEqual(textLabelKey, buttonMonoBehaviour.name);
        }

        [Test]
        public void TestChangeTextLabelText()
        {
            string key = "testTextLabel";
            string oldtext = "oldtext";
            string newtext = "newtext";
            graphicsModule.CreateButton(key, new Rect(1000, 500, 250, 250), oldtext, () => Debug.Log("clicked"));
            graphicsModule.SetButtonText(key, newtext);
            Assert.AreEqual(newtext, uiService.GetElementByKey(key).GetComponentInChildren<TextMeshProUGUI>().text);
        }

    }
}