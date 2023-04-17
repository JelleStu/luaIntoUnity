using System;
using System.Threading.Tasks;
using HamerSoft.Howl.Core;
using HamerSoft.Howl.Sharp;
using LuaBridge.Core.Abstract;
using LuaBridge.Core.Configuration;
using LuaBridge.Modules.Audio;
using LuaBridge.Unity.Scripts;
using LuaBridge.Unity.Scripts.LuaBridgeHelpers.JSonSerializer;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Services;
using LuaBridge.Unity.Scripts.LuaBridgesGames.Managers;
using Services;
using Services.Prefab;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GameModule.Unity.Scripts
{
    public class Main
    {
        private static bool _initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void SceneStarted()
        {
            if (_initialized)
                return;
            _initialized = true;
            if (SceneManager.GetActiveScene().name.Contains("test", StringComparison.InvariantCultureIgnoreCase))
                return;
            InitAppContainer();
        }

        private static async void InitAppContainer()
        {
            var sceneContainer = Object.FindObjectOfType<LuaBridgeSceneContainer>();
            var container = new AppConfiguration()
                .AddSingleton<IJsonSerializer, JsonSerializer>()
                .AddSingleton<IFileService, FileService>()
                .AddSingleton<IUIService, UGuiCanvasService>(sceneContainer.canvas)
                .AddSingleton<IPrefabService, PrefabService>("PrefabRegistry")
                .AddSingleton<AudioModule>()
                .AddSingleton<IApi, Api>()
                .AddSingleton<GameManager>()
                .Build()
                .Bind<AudioModule>((appContainer, module) => module.SetAudioService(appContainer.GetService<IFileService>()));

            
            await Task.WhenAll(
                container.GetService<IPrefabService>().Boot(),
                container.GetService<AudioModule>().Boot()
                );
            
            container.GetService<IUIService>().Boot();
            var gameManager = container.GetService<GameManager>(); 
            gameManager.Initialize();
        }
    }
}