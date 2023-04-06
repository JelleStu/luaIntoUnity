using LuaBridge.Core.Configuration;
using UnityEngine;

namespace LuaBridge.Unity.Scripts
{
    public class LuaBridgeSceneContainer : AbstractSceneContainer
    {
        private Canvas canvas;

        public void ContainerStart()
        {
            canvas = GetComponent<Canvas>();
        }
    }
}