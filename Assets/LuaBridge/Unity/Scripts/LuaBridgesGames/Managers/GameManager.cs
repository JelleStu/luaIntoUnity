using System;
using System.IO;
using HamerSoft.Howl.Core;
using HamerSoft.Howl.Sharp;
using LuaBridge.Unity.Scripts.LuaBridgeModules.GraphicsModule;
using LuaBridge.Unity.Scripts.LuaBridgeServices.UIService.Interface;
using LuaBridge.Unity.Scripts.LuaBridgesProxies.GraphicsService;
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
        private IMoonSharpApi _api;
        private readonly IUIService _canvasService;
        private ISandbox _sandbox;
        private UpdateLoop _updateloop;
        private bool _luaManagerInitialized = false;

        public GameManager(IMoonSharpApi api, IUIService canvasService)
        {
            _api = api;
            _canvasService = canvasService;
        }
        

        public void Initialize()
        {
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Action),
                v =>
                {
                    var function = v.Function;

                    return (Action) (() => function.Call());
                }
                
            );
            
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeof(Rect),
                v =>
                {
                    var luaTable = v.Table;
                    return new Rect(luaTable.Get("x").ToObject<float>(), luaTable.Get("y").ToObject<float>(), luaTable.Get("width").ToObject<float>(), luaTable.Get("height").ToObject<float>());
                }
                
            );
            _api.AddProxy(new GraphicsServiceProxy(new GraphicsModule(_canvasService)));
            _eventRaiser = new GameObject("UnityEvents").AddComponent<EventRaiser>();
            _updateloop = new GameObject("UpdateLoop").AddComponent<UpdateLoop>();
            _canvasService.Root.LoadSnakeGameButton.onClick.AddListener(LoadSnakeGame);
            _canvasService.Root.LoadTicTacToeGameButton.onClick.AddListener(LoadTicTacToeGame);
            _eventRaiser.Started += EventRaiserOnStarted_Handler;
            _updateloop.Updated += UpdateLoopOnUpdate_Handler;
            _eventRaiser.ApplicationQuitted += EventRaiserOnApplicationQuitted_Handler;
        }

        private void LoadSnakeGame()
        {
            _sandbox = _api.CreateSandBox(new SandboxConfig("SnakeGameSandBox", $"{Path.Combine(Application.streamingAssetsPath, "LuaGames", "Snake", "Scripts")}", $"SnakeGameManager.lua"));
            _canvasService.SetSandBoxRootDirectory($"{Path.Combine(Application.streamingAssetsPath, "LuaGames", "Snake")}");
            InitializeGame();
            AddStartGameButtonListener();
        }

        private void LoadTicTacToeGame()
        {
            _sandbox = _api.CreateSandBox(new SandboxConfig("TicTacToeGameSandBox", $"{Path.Combine(Application.streamingAssetsPath, "LuaGames", "TicTacToe")}", $"TicTacToeGameManager.lua"));
            InitializeGame();
            AddStartGameButtonListener();
        }

        private void AddStartGameButtonListener()
        {
            _canvasService.Root.StartGameButton.onClick.AddListener(()=> _sandbox.Invoke("Player:GameStart"));

        }

        private void EventRaiserOnApplicationQuitted_Handler()
        {
            _eventRaiser.ApplicationQuitted -= EventRaiserOnApplicationQuitted_Handler;
            _updateloop.Updated -= UpdateLoopOnUpdate_Handler;
            _sandbox?.Dispose();
            _api?.Dispose();
        }

        private void UpdateLoopOnUpdate_Handler()
        {
            if (_luaManagerInitialized)
                _sandbox.Invoke("Player:Update");
        }

        private void EventRaiserOnStarted_Handler()
        {
            _eventRaiser.Started -= EventRaiserOnStarted_Handler;
        }

        private void InitializeGame()
        {
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