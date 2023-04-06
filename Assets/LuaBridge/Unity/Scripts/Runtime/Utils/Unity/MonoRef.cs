using UnityEngine;

namespace Utils.Unity
{
    public class MonoRef : EventRaiser
    {
        private void OnApplicationQuit()
        {
            StopAllCoroutines();
            DestroyImmediate(gameObject);
            _instance = null;
        }

        private static MonoRef _instance;

        public static MonoRef Instantiate()
        {
            return _instance != null ? _instance : (_instance = new GameObject("MonoRef").AddComponent<MonoRef>());
        }
    }
}