using System;

namespace Utils.Caching.Abstract
{
    public interface ICache<T> : IDisposable
    {
        public void Add(string id, T o);
        public bool Exists(string id);
        public bool TryGet(string id, out T cache);
        public object Get(string id);
        public bool TryGet<T1>(string id, out T1 cache);
        public T1 Get<T1>(string id);

        public void Clear();
        public void Delete(string id);
        public void Delete(Predicate<T> deleteFilter);
    }
}