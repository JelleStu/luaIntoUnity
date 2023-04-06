using System;
using UnityEngine;

namespace LuaBridge.Core.Utils.Threading
{
    /// <summary>
    /// Monobehaviour that raises the update event from unity
    /// </summary>
    public class UpdateLoop : MonoBehaviour
    {
        public event Action Updated;
        public event Action Destroyed;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            Updated?.Invoke();
        }

        private void OnApplicationQuit()
        {
            Destroyed?.Invoke();
        }
    }
}