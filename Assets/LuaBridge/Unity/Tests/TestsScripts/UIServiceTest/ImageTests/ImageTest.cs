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
using LuaBridge.Unity.Tests.Tests.UIServiceTest.Extensions;
using NUnit.Framework;
using Services;
using Services.Prefab;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace LuaBridge.Unity.Tests.UIServiceTest
{
    public class ImageTest : LuaBridgeTest
    {
        private IUIService uiService;
        private GraphicsModule graphicsModule;
        private Canvas canvas;
        private readonly string _key = "testImage";
        private readonly string _imageNameDoesntExist = "plup.png";
        private readonly string _imageOneName = $"test_image_1.png";
        private readonly string _imageTwoName = $"test_image_2.png";
        private string _sandBoxRootDirectory = Path.Combine($"{Application.streamingAssetsPath}", "LuaGames", "TestGame");
        
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            canvas = new GameObject("Canvas").AddComponent<Canvas>();
            yield return base.SetUp();
            uiService = AppContainer.GetService<IUIService>();
            graphicsModule = new GraphicsModule(uiService);
            uiService.SetSandBoxRootDirectory(_sandBoxRootDirectory);
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

        [UnityTest]
        public IEnumerator Create_Image_On_Canvas()
        {
            yield return graphicsModule.CreateImage(_key, new Rect(1000, 500, 250, 250), $"{_imageOneName}").ToCoroutine();
            var image = canvas.GetComponentInChildren<JeltieboyImage>();
            Assert.NotNull(image);
        }
        
        [UnityTest]
        public IEnumerator Create_Sprite()
        {
            yield return graphicsModule.CreateImage(_key, new Rect(1000, 500, 250, 250), $"{_imageOneName}").ToCoroutine();
            var image = canvas.GetComponentInChildren<JeltieboyImage>();
            Assert.NotNull(image.Image.sprite);
        }

        [UnityTest]
        public IEnumerator Change_Sprite()
        {
            yield return graphicsModule.CreateImage(_key, new Rect(1000, 500, 250, 250), $"{_imageOneName}").ToCoroutine();
            JeltieboyImage image = (JeltieboyImage)graphicsModule.GetElementByKey(_key);
            yield return graphicsModule.ChangeImage(_key, _imageTwoName).ToCoroutine();
            JeltieboyImage imageTwo = (JeltieboyImage)graphicsModule.GetElementByKey(_key);
            Assert.AreNotSame(image.Image.sprite.texture.GetPixels(), imageTwo.Image.sprite.texture.GetPixels());
        }
        
        [UnityTest]
        public IEnumerator Create_Sprite_Source_Not_Exist()
        {
            LogAssert.ignoreFailingMessages = true;
            string pathnotfound = Path.Combine(_sandBoxRootDirectory, "Assets", "Images", _imageNameDoesntExist);
            yield return graphicsModule.CreateImage(_key, new Rect(1000, 500, 250, 250), $"{_imageNameDoesntExist}").ToCoroutine();
            LogAssert.Expect(LogType.Error, $"Can not find image with path {pathnotfound}");
        }
        
        [UnityTest]
        public IEnumerator Change_Sprite_Source_Not_Exist()
        {
            LogAssert.ignoreFailingMessages = true;
            string pathnotfound = Path.Combine(_sandBoxRootDirectory, "Assets", "Images", _imageNameDoesntExist);
            yield return graphicsModule.CreateImage(_key, new Rect(1000, 500, 250, 250), $"{_imageOneName}").ToCoroutine();
            yield return graphicsModule.ChangeImage(_key, _imageNameDoesntExist).ToCoroutine();
            LogAssert.Expect(LogType.Error, $"Can not find image with path {pathnotfound}");
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