using System;
using System.Collections.Generic;
using System.IO;
using Lunacy.Modules.Audio;
using Lunacy.Proxies.Audio;
using Lunacy.Proxies.EventBus;
using Modules;
using Modules.Graphics;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using MoonSharp.VsCodeDebugger;
using Proxies.GraphicsModule;
using UnityEngine;

namespace Luncay.Core
{
    public class FakeCoreModule : MonoBehaviour
    {
        public bool enableMovement;
        public static FakeCoreModule fakeCoreModule;
        private MoonSharpVsCodeDebugServer _server;
        private FileSystemScriptLoader _systemScriptLoader = null;
        private Script _someLuaScript;
        private DynValue _spawnButtonLuaFunction;
        private Table _functionTable;
        private DynValue _movementfunction;
        private DynValue _updateFunction;
        bool initialized = false;
        private static List<string> keyLists = new List<string>();
        
        private void Update()
        {
            if (enableMovement)
            {
                foreach (var key in keyLists)
                {
                    _someLuaScript.Call(_movementfunction, _functionTable, key);
                }
            }

            if (initialized)
                _someLuaScript.Call(_updateFunction);
        }

        private void Start()
        {
            Debug.unityLogger.logEnabled = false;
            fakeCoreModule = this;
            _server = new MoonSharpVsCodeDebugServer();
            _server.Start();
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Action),
                v =>
                {
                    var function = v.Function;

                    return (Action)(() => function.Call());
                }
            );
            Dephion.Core.Events.EventBus.Factory.Create();
            _systemScriptLoader = CreateFileSystemScriptLoader();
            LoadScript();
        }


        public void invokelua()
        {
            _someLuaScript.Call(_spawnButtonLuaFunction, new object[]{5});
            // _spawnButtonLuaFunction.Function.Call(DynValue.NewNumber(10));
        }

        public void ReloadScript()
        {
            LoadScript();
        }

        private void LoadScript()
        {
            _someLuaScript = new Script();
            _server.AttachToScript(_someLuaScript, "test123");
            _someLuaScript.Options.DebugPrint = Debug.Log;
            _someLuaScript.Options.ScriptLoader = CreateFileSystemScriptLoader();
            RegisterProxies();
            _someLuaScript.DoFile("Assets/Player.lua");
            _someLuaScript.Globals["EventBusProxy"] = new EventBus.EventBus();
            _someLuaScript.Globals["AudioModuleProxy"] = new AudioModule();
            _someLuaScript.Globals["GraphicsModuleProxy"] = new GraphicsModule();
            _functionTable = _someLuaScript.Globals["Player"] as Table;

            _movementfunction = _functionTable?.Get("SetButtonToRandomLocation");
            _updateFunction = _functionTable?.Get("Update");
            
            _spawnButtonLuaFunction = _functionTable?.Get("SpawnMultipleButtons");
            
            DynValue initializeScript = _functionTable?.Get("Initialize");

            var f = _functionTable?.Get("SpawnMultipleButtons");
            
            var _initialized = _someLuaScript.Call(initializeScript);

            _someLuaScript.Globals["AllButtonsSpawned"] = (Func<int>)Mul;
            
            _someLuaScript.Call(_spawnButtonLuaFunction, new object[]{_functionTable,200});
            initialized = _initialized.Boolean;
        }

        private int Mul()
        {
            keyLists = CanvasManager.instance.GetAllKeys();
            return keyLists.Count;
        }

        private FileSystemScriptLoader CreateFileSystemScriptLoader()
        {
            return new FileSystemScriptLoader()
            {
                ModulePaths = new string[] {Path.Combine($"{Application.dataPath}/LuaModules", "?.lua"),
                    Path.Combine($"{Application.dataPath}/LuaModules/Graphics", "?.lua"), 
                    Path.Combine($"{Application.dataPath}/LuaModules/Audio", "?.lua"),
                    Path.Combine($"{Application.dataPath}/LuaModules/EventBus", "?.lua")
                }
            };
        }

        private void RegisterProxies()
        {
            UserData.RegisterProxyType<EventBusProxy, EventBus.EventBus>(eventBus => new EventBusProxy(eventBus));
            UserData.RegisterProxyType<AudioModuleProxy, AudioModule>(audioModule => new AudioModuleProxy(audioModule));
            UserData.RegisterProxyType<GraphicsModuleProxy, GraphicsModule>(graphicsModule => new GraphicsModuleProxy(graphicsModule));

        }

        public void StartTheMovement()
        {
        }
    }
}