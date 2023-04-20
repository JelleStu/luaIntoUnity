using System.Collections;
using System.IO;
using System.Threading.Tasks;
using LuaBridge.Core.Abstract;
using LuaBridge.Core.Configuration;
using LuaBridge.Unity.Scripts.LuaBridgeHelpers.JSonSerializer;
using LuaBridge.Unity.Scripts.LuaBridgeModules.GraphicsModule;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Services;
using LuaBridge.Unity.Tests.Tests.UIServiceTest.Abstract;
using NUnit.Framework;
using Services;
using Services.Prefab;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace LuaBridge.Unity.Tests.Tests.UIServiceTest.ImageTests
{
    public class ImageTest : LuaBridgeTest
    {
        private IUIService uiService;
        private GraphicsModule graphicsModule;
        private Canvas canvas;
        
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            canvas = new GameObject("Canvas").AddComponent<Canvas>();
            yield return base.SetUp();
            uiService = AppContainer.GetService<IUIService>();
            graphicsModule = new GraphicsModule(uiService);
        }

        protected override async Task<AppContainer> CreateContainer()
        {
            var container = new AppConfiguration()
                .AddSingleton<IJsonSerializer, JsonSerializer>()
                .AddSingleton<IFileService, FileService>()
                .AddSingleton<IUIService, UGuiCanvasService>(canvas)
                .AddSingleton<IPrefabService, PrefabService>("PrefabRegistry")
                .Build()
                .Bind<IUIService>((appContainer, module) => module.SetFileService(appContainer.GetService<IFileService>()));


            await Task.WhenAll(
                container.GetService<IPrefabService>().Boot());
            container.GetService<IUIService>().Boot();

            return container;
        }

        [Test]
        public void Create_Image_On_Canvas()
        {
            string key = "testImage";
            Debug.Log($"{Path.Combine(Application.persistentDataPath, "apple.png")}");
            graphicsModule.CreateImage(key, new Rect(1000, 500, 250, 250), $"{Path.Combine(Application.persistentDataPath, "apple.png")}");
            var image = canvas.GetComponentInChildren<JeltieboyImage>();
            Assert.AreEqual(1, uiService.GetAllKeys().Count);
            Assert.NotNull(image);

        }
        
        
        
        
        

        [UnityTearDown]
        public override IEnumerator TearDown()
        {
            AppContainer.Dispose();
            AppContainer = null;
            yield return null;
        }
        
    }
}