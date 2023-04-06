using LuaBridge.Core.Abstract;
using LuaBridge.Core.Configuration;
using LuaBridge.Unity.Scripts.LuaBridgeHelpers.JSonSerializer;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService;
using LuaBridge.Unity.Scripts.LuaBridgesGames.Managers;
using Services;
using UnityEngine;

namespace GameModule.Unity.Scripts
{
    public class Main
    {
        private static bool _initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SceneStarted()
        {
            if (_initialized)
                return;
            _initialized = true;
            InitAppContainer();
        }

        private static void InitAppContainer()
        {
            var container = new AppConfiguration();
            container.AddSingleton<IJsonSerializer, JsonSerializer>();
            container.AddSingleton<IFileService, FileService>();
            container.AddSingleton<IUIService, UGuiService>(Object.FindObjectOfType(typeof(Canvas)));
             var appContainer = container.Build();
             GameManager gameManager = new GameManager(appContainer);
             gameManager.Initialize();
        }
        
    }
}
