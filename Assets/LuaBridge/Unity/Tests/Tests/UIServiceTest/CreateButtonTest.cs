using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuaBridge.Core.Configuration;
using LuaBridge.Core.Services.Abstract;
using LuaBridge.Unity.Scripts.LuaBridgeModules.GraphicsModule;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Services;
using NUnit.Framework;
using Services.Prefab;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace LuaBridge.Unity.Scripts.Tests.Tests.UIServiceTest
{
    public static class TaskExtensions
    {
        public static IEnumerator ToCoroutine(this Task task)
        {
            while (!task.IsCompleted || task.Status == TaskStatus.WaitingForActivation)
                yield return null;
        }

        public static IEnumerator ToCoroutine<T>(this Task<T> task, Action<T> callback)
        {
            while (!task.IsCompleted || task.Status == TaskStatus.WaitingForActivation)
                yield return null;
            callback?.Invoke(task.Result);
        }
    }

    public abstract class LuaBridgeTest
    {
        protected AppContainer AppContainer;


        [UnitySetUp]
        public virtual IEnumerator SetUp()
        {
            yield return CreateContainer().ToCoroutine(container => AppContainer = container);
        }

        protected abstract Task<AppContainer> CreateContainer();

        [UnityTearDown]
        public virtual IEnumerator TearDown()
        {
            AppContainer.Dispose();
            yield return null;
        }

    }

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
    }
}