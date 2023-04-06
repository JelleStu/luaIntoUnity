using HamerSoft.Howl.Core;
using HamerSoft.Howl.Sharp;
using HamerSoft.Howl.Sharp.Locators;
using LuaBridge.Core.Configuration;
using UnityEngine;

namespace LuaBridge.Unity.Scripts.LuaBridgesGames.Managers
{
    public class GameManager
    {
        private AppContainer appContainer;
        
        
        public GameManager(AppContainer _appContainer)
        {
            appContainer = _appContainer;
        }

        public void Initialize()
        {
            //Get script from somewhere?

            Api api = new Api();
            var sandbox = api.CreateSandBox(new SandboxConfig("GeneralGameSandbox", Application.streamingAssetsPath, GetMainLuaScriptFile()));
            sandbox.AddProxy();
            sandbox.Start();
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