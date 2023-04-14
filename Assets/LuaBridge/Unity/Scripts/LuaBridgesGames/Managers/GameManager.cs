using System;
using System.IO;
using HamerSoft.Howl.Core;
using LuaBridge.Unity.Scripts.LuaBridgeModules.GraphicsModule;
using LuaBridge.Unity.Scripts.LuabridgeProxies.GraphicsService;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface;
using MoonSharp.Interpreter;
using UnityEngine;
using Utils.Unity;
using Script = MoonSharp.Interpreter.Script;
using UpdateLoop = LuaBridge.Core.Utils.Threading.UpdateLoop;

namespace LuaBridge.Unity.Scripts.LuaBridgesGames.Managers
{
    public class GameManager
    {
        private EventRaiser _eventRaiser;
        private IApi _api;
        private readonly IUIService _canvasService;
        private ISandbox _sandbox;
        private UpdateLoop _updateloop;
        private bool _luaManagerInitialized = false;

        public GameManager(IApi api, IUIService canvasService)
        {
            _api = api;
            _canvasService = canvasService;
        }
        

        public void Initialize()
        {
            Application.targetFrameRate = 80;
            _sandbox = _api.CreateSandBox(new SandboxConfig("GeneralGameSandbox", $"{Path.Combine(Application.streamingAssetsPath, "LuaModules")}", $"Player"));
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Action),
                v =>
                {
                    var function = v.Function;

                    return (Action) (() => function.Call());
                }
                
            );
            _api.AddProxy(new GraphicsServiceProxy(new GraphicsModule(_canvasService)));
            _eventRaiser = new GameObject("UnityEvents").AddComponent<EventRaiser>();
            _updateloop = new GameObject("UpdateLoop").AddComponent<UpdateLoop>();
            _eventRaiser.Started += EventRaiserOnStarted_Handler;
            _updateloop.Updated += UpdateLoopOnUpdate_Handler;
            
        }

        private void UpdateLoopOnUpdate_Handler()
        {
            if (_luaManagerInitialized)
                _sandbox.Invoke("Player:Update");
        }

        private void EventRaiserOnStarted_Handler()
        {
            _eventRaiser.Started += EventRaiserOnStarted_Handler;
            _sandbox.Start();
            _sandbox.Invoke("Player:Initialize", null, (Action) InitializeCallback);
        }

        private void InitializeCallback()
        {
            _luaManagerInitialized = true;
        }


        /// <summary>
        /// Gets the Main Lua script file.
        /// Normally this should be downloaded.
        /// </summary>
        private string GetMainLuaScriptFile()
        {
            return $"{Application.streamingAssetsPath}/Player.lua";
        }
    }
}