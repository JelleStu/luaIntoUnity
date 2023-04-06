using System.ComponentModel.Design;
using UnityEngine;

namespace LuaBridge.Core.Services.Abstract
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Variables

        public static T Instance => GetInstance();

        private static MonoBehaviour _instance;

        #endregion

        #region Unity Methods

        protected virtual void OnDestroy()
        {
            _instance = null;
        }

        private static T GetInstanceBySceneObject()
        {
            _instance = FindObjectOfType<T>();
            return (T)_instance;
        }

        private static T GetInstanceByInstantiating()
        {
            var singletonObject = new GameObject();
            _instance = singletonObject.AddComponent<T>();
            singletonObject.name = typeof(T).Name;
            return (T)_instance;
        }

        private static T GetInstance()
        {
            if (_instance == null || _instance.gameObject == null)
                _instance = GetInstanceBySceneObject();

            // Only instantiate gameobjects runtime
            if (!Application.isPlaying && _instance == null)
                return default;

            if (_instance == null || _instance.gameObject == null && typeof(T) != typeof(ServiceContainer))
                _instance = GetInstanceByInstantiating();

            return (T)_instance;
        }

        #endregion
    }
}