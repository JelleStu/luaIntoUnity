using System;
using MoonSharp.Interpreter.Interop;

namespace Luncay.Core
{
    public class ProxyFactory : IProxyFactory
    {
        public Type TargetType => _targetType;
        public Type ProxyType => _proxyType;

        private readonly Type _targetType;
        private readonly Type _proxyType;
        private readonly Func<object, object> _creationMethod;

        public ProxyFactory(Type targetType, Type proxyType, Func<object, object> creationMethod)
        {
            _targetType = targetType;
            _proxyType = proxyType;
            _creationMethod = creationMethod;
        }

        public object CreateProxyObject(object o)
        {
            return _creationMethod.Invoke(o);
        }
    }
}