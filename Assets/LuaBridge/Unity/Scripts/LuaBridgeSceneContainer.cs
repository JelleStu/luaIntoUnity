using LuaBridge.Core.Configuration;
using LuaBridge.Unity.Scripts.LuaBridgeHelpers.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace LuaBridge.Unity.Scripts
{
    public class LuaBridgeSceneContainer : AbstractSceneContainer
    {
        public Canvas canvas;
        public AudioSource audioSource;
        public SwipeManager swipeManager;
    }
}