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

    public abstract class LuaBridgeTest
    {
        protected AppContainer AppContainer;
        [OneTimeSetUp]
        public virtual void OneTimeSetup()
        {
            AppContainer = CreateContainer().Result;
        }
        
        
        protected abstract Task<AppContainer> CreateContainer();

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            AppContainer.Dispose();
        }

    }
    public class CreateButtonTest : LuaBridgeTest
    {
        protected Canvas Canvas;
        private GraphicsModule graphicsModule;
        private IUIService uiService;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return new EnterPlayMode();
        }
        [OneTimeSetUp]
        public override void OneTimeSetup()
        { 
            SceneManager.LoadScene(1);
            Canvas = new GameObject("Canvas").AddComponent<Canvas>();
            IEnumerator WaitForOurTask() {
                Task task = CreateContainer();
                yield return new WaitUntil(() => task.IsCompleted);
            }

            var waitForOurTask = WaitForOurTask();
            
            base.OneTimeSetup();
            uiService = AppContainer.GetService<IUIService>();
            graphicsModule = new GraphicsModule(uiService);
       }

       protected override Task<AppContainer> CreateContainer()
       {
           var container = new AppConfiguration()
               .AddSingleton<IUIService, UGuiCanvasService>(Canvas)
               .AddSingleton<IPrefabService, PrefabService>("PrefabRegistry")
               .Build();

            Task.WhenAll(
                    container.GetService<IPrefabService>().Boot());
            return Task.FromResult(container);
       }
       
        [Test]
        public void TestSpawnButton()
        {
            graphicsModule.CreateButton("test", new Vector2(1000, 500), 250, 250, () => Debug.Log("clicked") );
            Assert.Positive(uiService.GetAllKeys().Count);
        }

        [UnityTest]
        public IEnumerator TestFoo()
        {
            Button button = null;
            yield return new WaitUntil(() => button!= null);
            
            Assert.NotNull(button);
            yield return new WaitForEndOfFrame();
        }
        
    }
}