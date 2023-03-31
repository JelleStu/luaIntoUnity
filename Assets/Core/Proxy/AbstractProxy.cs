using System;

namespace Luncay.Core
{
    public abstract class AbstractProxy
    {
        public string LuaName { get; }
        public Type CLRType { get; }

        public abstract object CreationMethod(object o);
    }
}