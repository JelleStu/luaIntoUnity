using System;
using System.Collections.Generic;

namespace LuaBridge.Core.Configuration
{
    public class AppContainer : IDisposable
    {
        private Dictionary<Type, object> _services;

        public AppContainer(Dictionary<Type, object> services)
        {
            _services = services;
        }

        public AppContainer Bind<T>(Action<AppContainer, T> binding)
        {
            if (!_services.TryGetValue(typeof(T), out var s))
                throw new Exception($"Failed to Bind for Type: {typeof(T)}. No registered type found!");
            binding.Invoke(this, (T)s);
            return this;
        }

        public T GetService<T>()
        {
            _ = TryGetService<T>(out var s);
            return s;
        }

        public bool TryGetService<T>(out T service)
        {
            service = default;
            var result = _services.TryGetValue(typeof(T), out var s);
            service = (T)s;
            return result;
        }
        public void Dispose()
        {
            foreach (KeyValuePair<Type, object> service in _services)
                if (service.Value is IDisposable d)
                    d.Dispose();

            _services = new Dictionary<Type, object>();
        }
    }
}