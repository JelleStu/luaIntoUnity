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
using UnityEngine.UI;

namespace LuaBridge.Unity.Tests.Tests.UIServiceTest.ButtonTest
{
 public class CreateButtonTest : LuaBridgeTest
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
        public void TestSpawnButton()
        {
            graphicsModule.CreateButton("test", new Vector2(1000, 500), 250, 250, "test", () => Debug.Log("clicked"));
            Assert.AreEqual(1, uiService.GetAllKeys().Count);
        }
        
        [Test]
        public void TestSpawnButtonGetWithName()
        {
            string buttonName = "test";
            graphicsModule.CreateButton(buttonName, new Vector2(1000, 500), 250, 250, "test", () => Debug.Log("clicked"));
            var buttonMonoBehaviour = uiService.GetElementByKey(buttonName);
            Assert.AreEqual(buttonName, buttonMonoBehaviour.name);
        }

        [Test]
        public void TestOnclickAction()
        {
            graphicsModule.CreateButton("test", new Vector2(1000, 500), 250, 250, "test", () => Debug.Log("clicked"));
            Button button = (Button) graphicsModule.GetElementByKey("test");
            button.onClick.Invoke();
            LogAssert.Expect(LogType.Log, "clicked");
        }
        
        [Test]
        public void TestChangeButtonText()
        {
            string oldtext = "oldtext";
            string newtext = "newtext";
            graphicsModule.CreateButton("test", new Vector2(1000, 500), 250, 250, oldtext, () => Debug.Log("clicked"));
            graphicsModule.SetButtonText("test", newtext);
            Assert.AreEqual(newtext, uiService.GetElementByKey("test").GetComponentInChildren<TextMeshProUGUI>().text);
        }
    }
}