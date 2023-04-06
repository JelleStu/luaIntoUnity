using System;
using LuaBridge.Core.Services.Abstract;
using UnityEngine;

namespace Services.Prefab
{
    public interface IPrefabService : IAsyncBootService
    {
        public T GetPrefab<T>(Predicate<T> search = null) where T : MonoBehaviour;
        public T GetPrefab<T>(string id, Predicate<T> search = null) where T : MonoBehaviour, IIdentifiablePrefab;
        public T SpawnPrefab<T>(Predicate<T> search = null, Transform parent = null) where T : MonoBehaviour;
        public T SpawnPrefab<T>(string id, Predicate<T> search = null, Transform parent = null) where T : MonoBehaviour, IIdentifiablePrefab;

        public T FindInstance<T>(Predicate<T> search = null) where T : MonoBehaviour;
        public T FindInstance<T>(string id, Predicate<T> search = null) where T : MonoBehaviour, IIdentifiablePrefab;

        public void RegisterInstance(MonoBehaviour o);
        public void RemoveInstance(MonoBehaviour o);
    }
}