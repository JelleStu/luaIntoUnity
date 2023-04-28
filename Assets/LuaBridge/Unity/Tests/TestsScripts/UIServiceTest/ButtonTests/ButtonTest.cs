using System.Collections;
using System.Text.RegularExpressions;
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

namespace LuaBridge.Unity.Tests.UIServiceTest
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
        public void Test_SpawnButton()
        {
            graphicsModule.CreateButton("test", new Rect(1000, 500, 250 , 250), "test", () => Debug.Log("clicked"));
            Assert.AreEqual(1, uiService.GetAllKeys().Count);
        }
        
        [Test]
        public void Test_Spawn_Button_Get_With_Name()
        {
            string buttonName = "test";
            graphicsModule.CreateButton(buttonName, new Rect(1000, 500, 250 , 250), "test", () => Debug.Log("clicked"));
            var buttonMonoBehaviour = uiService.GetElementByKey(buttonName);
            Assert.AreEqual(buttonName, buttonMonoBehaviour.name);
        }

        [Test]
        public void Test_On_Click_Action()
        {
            graphicsModule.CreateButton("test", new Rect(1000, 500, 250 , 250), "test", () => Debug.Log("clicked"));
            Button button = (Button) graphicsModule.GetElementByKey("test");
            button.onClick.Invoke();
            LogAssert.Expect(LogType.Log, "clicked");
        }
        
        [Test]
        public void Test_Change_Button_Text()
        {
            string key = "testButton";
            string oldtext = "oldtext";
            string newtext = "newtext";
            graphicsModule.CreateButton(key, new Rect(1000, 500, 250 , 250), oldtext, () => Debug.Log("clicked"));
            graphicsModule.SetButtonText(key, newtext);
            Assert.AreEqual(newtext, uiService.GetElementByKey(key).GetComponentInChildren<TextMeshProUGUI>().text);
        }

        [Test]
        public void Test_Try_Change_Button_Wrong_Key()
        {
            string nonExistingKey = "doesnotexist";
            graphicsModule.SetButtonText(nonExistingKey, "newtext");
            LogAssert.Expect(LogType.Error, new Regex($"Can not find button with key {nonExistingKey}"));
        }
    }
}