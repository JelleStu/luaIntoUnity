using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Awaiters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services.Prefab
{
    public class PrefabService : IPrefabService
    {
        private readonly string _registryPath;
        private IPrefabRegistry _registry;
        private List<MonoBehaviour> _instances;

        public PrefabService(string registryPath)
        {
            _registryPath = registryPath;
            _instances = new List<MonoBehaviour>();
        }

        public void Dispose()
        {
        }

        public async Task Boot()
        {
            _registry = (await Resources.LoadAsync(_registryPath)) as IPrefabRegistry;
        }

        public T GetPrefab<T>(Predicate<T> search = null) where T : MonoBehaviour
        {
            return _registry?.Prefabs.FirstOrDefault(p => p.TryGetComponent<T>(out var c) && (search == null || search.Invoke(c))) as T;
        }

        public T GetPrefab<T>(string id, Predicate<T> search = null) where T : MonoBehaviour, IIdentifiablePrefab
        {
            return _registry?.Prefabs.FirstOrDefault(p => p.TryGetComponent<T>(out var c) && c.Id == id && (search == null || search.Invoke(c))) as T;
        }

        public T SpawnPrefab<T>(Predicate<T> search = null, Transform parent = null) where T : MonoBehaviour
        {
            T instance = default;
            var prefab = GetPrefab(search);
            if (prefab != null)
            {
                instance = Object.Instantiate(prefab, parent);
                _instances.Add(instance );
            }

            return instance;
        }

        public T SpawnPrefab<T>(string id, Predicate<T> search = null, Transform parent = null) where T : MonoBehaviour, IIdentifiablePrefab
        {
            T instance = default;
            var prefab = GetPrefab(id, search);
            if (prefab != null)
            {
                instance = Object.Instantiate(prefab, parent);
                _instances.Add(instance);
            }

            return instance;
        }

        public T FindInstance<T>(Predicate<T> search = null) where T : MonoBehaviour
        {
            for (int i = 0; i < _instances.Count; i++)
            {
                if (_instances[i] == null)
                {
                    _instances.RemoveAt(i);
                    continue;
                }

                if (_instances[i] is T t && (search == null || search.Invoke(t)))
                    return t;
            }

            return default;
        }

        public T FindInstance<T>(string id, Predicate<T> search = null) where T : MonoBehaviour, IIdentifiablePrefab
        {
            for (int i = 0; i < _instances.Count; i++)
            {
                if (_instances[i] == null)
                {
                    _instances.RemoveAt(i);
                    continue;
                }

                if (_instances[i] is T t && t.Id == id && (search == null || search.Invoke(t)))
                    return t;
            }

            return default;
        }

        public void RegisterInstance(MonoBehaviour o)
        {
            _instances.Add(o);
        }

        public void RemoveInstance(MonoBehaviour o)
        {
            _instances.Remove(o);
        }
    }
}