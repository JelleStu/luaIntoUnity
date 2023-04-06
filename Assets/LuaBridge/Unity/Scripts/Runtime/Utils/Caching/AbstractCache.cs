using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services;
using Utils.Caching.Abstract;

namespace Utils.Caching
{
    public abstract class AbstractCache<T> : ICache<T>
    {
        protected ConcurrentDictionary<string, T> Cache;

        public AbstractCache()
        {
            Cache = new ConcurrentDictionary<string, T>();
        }

        public AbstractCache(Dictionary<string, T> source)
        {
            Cache = new ConcurrentDictionary<string, T>(source ?? new Dictionary<string, T>());
            source = null;
        }

        public void Add(string id, T o)
        {
            Cache[id] = o;
        }

        public bool Exists(string id)
        {
            return Cache.ContainsKey(id);
        }

        public bool TryGet(string id, out T cache)
        {
            cache = default;
            return !string.IsNullOrWhiteSpace(id) && Cache.TryGetValue(id, out cache);
        }

        public object Get(string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && Cache.TryGetValue(id, out var cache))
                return cache;
            return null;
        }

        public bool TryGet<T1>(string id, out T1 cache)
        {
            cache = default;
            if (TryGet(id, out var c) && c is T1 result)
                cache = result;
            return cache != null;
        }

        public T1 Get<T1>(string id)
        {
            if (TryGet<T1>(id, out var result))
                return result;
            return default;
        }

        public virtual void Clear()
        {
            foreach (var c in Cache)
                if (c.Value is IDisposable disposable)
                    disposable.Dispose();
            Cache = new ConcurrentDictionary<string, T>();
        }

        public void Delete(string id)
        {
            T reference = default;
            if (!string.IsNullOrWhiteSpace(id) && Cache?.TryRemove(id, out reference) == false)
                return;
            if (reference is IDisposable disposable)
                disposable.Dispose();
        }

        public void Delete(Predicate<T> deleteFilter)
        {
            var matches = Cache.Where(c => deleteFilter(c.Value)).ToArray();
            for (int i = 0; i < matches.Length; i++)
                Delete(matches[i].Key);
        }

        public virtual void Dispose()
        {
            Clear();
            Cache = null;
        }
    }

    public static class CacheExtensions
    {
        public static async Task WriteToDisk<T>(this AbstractCache<T> cache, string path, IFileService fileService)
        {
            await fileService.SaveJson(cache, path);
        }

        public static async Task LoadFromDisk<T>(this AbstractCache<T> cache, string path, IFileService fileService)
        {
            cache = await fileService.LoadJson<AbstractCache<T>>(path);
        }
    }
}