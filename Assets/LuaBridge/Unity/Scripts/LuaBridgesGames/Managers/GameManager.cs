using System;
using System.IO;
using HamerSoft.Howl.Core;
using HamerSoft.Howl.Sharp;
using LuaBridge.Core.Configuration;
using Modules;
using Modules.Graphics;
using MoonSharp.Interpreter;
using Proxies.GraphicsModule;
using UnityEngine;
using Utils.Unity;
using Script = MoonSharp.Interpreter.Script;

namespace LuaBridge.Unity.Scripts.LuaBridgesGames.Managers
{
    public class GameManager
    {
        private EventRaiser _eventRaiser;
        private IApi _api;
        private readonly CanvasManager _canvasManager;
        private ISandbox _sandbox;


        public GameManager(IApi api, CanvasManager canvasManager)
        {
            _api = api;
            _canvasManager = canvasManager;
        }

        public void Initialize()
        {
            //Get script from somewhere?

            _sandbox = _api.CreateSandBox(new SandboxConfig("GeneralGameSandbox", $"{Path.Combine(Application.streamingAssetsPath, "LuaModules")}", $"Player"));
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Action),
                v =>
                {
                    var function = v.Function;

                    return (Action) (() => function.Call());
                }
            );
            _api.AddProxy(new GraphicsModuleProxy(new GraphicsModule(_canvasManager)));

            _eventRaiser = new GameObject("UnityEvents").AddComponent<EventRaiser>();
            _eventRaiser.Started += EventRaiserOnStarted_Handler;
        }

        private void EventRaiserOnStarted_Handler()
        {
            _eventRaiser.Started += EventRaiserOnStarted_Handler;
            _sandbox.Start();
            _sandbox.Invoke("Player:Initialize");
            _sandbox.Invoke("Player:SpawnMultipleButtons", "Player", 10, (Action) test);
        }

        private void test()
        {
            return;
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