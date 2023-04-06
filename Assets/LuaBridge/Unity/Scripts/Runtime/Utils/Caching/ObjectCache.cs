using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Utils.Caching.Abstract;
using Object = UnityEngine.Object;

namespace Utils.Caching
{
    public class ObjectCache : ICache<Object>
    {
        private ConcurrentDictionary<string, Object> _cache;

        public ObjectCache()
        {
            _cache = new ConcurrentDictionary<string, Object>();
        }

        public ObjectCache(Dictionary<string, Object> source)
        {
            _cache = new ConcurrentDictionary<string, Object>(source ?? new Dictionary<string, Object>());
            source = null;
        }

        public void Add(string id, Object o)
        {
            _cache[id] = o;
        }

        public bool Exists(string id)
        {
            return _cache.ContainsKey(id);
        }

        public bool TryGet(string id, out Object cache)
        {
            cache = null;
            return !string.IsNullOrWhiteSpace(id) && _cache.TryGetValue(id, out cache);
        }

        public object Get(string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && _cache.TryGetValue(id, out var cache))
                return cache;
            return null;
        }

        public bool TryGet<T>(string id, out T cache)
        {
            cache = default;
            if (TryGet(id, out var c) && c is T result)
                cache = result;
            return cache != null;
        }

        public T Get<T>(string id)
        {
            if (TryGet<T>(id, out var result))
                return result;
            return default;
        }

        public void Clear()
        {
            foreach (var o in _cache)
            {
                if (o.Value is IDisposable disposable)
                    disposable.Dispose();
                Object.Destroy(o.Value);
            }

            _cache = new ConcurrentDictionary<string, Object>();
        }

        public void Delete(string id)
        {
            Object reference = null;
            if (!string.IsNullOrWhiteSpace(id) && _cache?.TryRemove(id, out reference) == false)
                return;
            if (reference is IDisposable disposable)
                disposable.Dispose();
            Object.Destroy(reference);
        }

        public void Delete(Predicate<Object> deleteFilter)
        {
            var matches = _cache.Where(c => deleteFilter(c.Value)).ToArray();
            for (int i = 0; i < matches.Length; i++)
                Delete(matches[i].Key);
        }

        public void Dispose()
        {
            Clear();
            _cache = null;
        }
    }
}