﻿using System;
using System.IO;
using Lunacy.Modules.Audio;
using Lunacy.Proxies.Audio;
using Lunacy.Proxies.EventBus;
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
        private Table functionTable;
        private DynValue _movementfunction;

        public DynValue GetLuaMoveButtonFunction()
        {
            return functionTable?.Get("SetButtonToRandomLocation");

        }

        private void Update()
        {
            if (enableMovement)
            {
                _someLuaScript.Call(_movementfunction);
            }
        }

        private void Start()
        {
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
            _someLuaScript.Call(_spawnButtonLuaFunction, int(10));
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
            functionTable = _someLuaScript.Globals["Player"] as Table;

            _movementfunction = functionTable?.Get("SetButtonToRandomLocation");
            
            _spawnButtonLuaFunction = functionTable?.Get("SpawnMultipleButtons");
            DynValue initializeScript = functionTable?.Get("Initialize");
            _someLuaScript.Call(initializeScript);
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