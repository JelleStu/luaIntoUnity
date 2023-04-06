using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LuaBridge.Core.Extensions
{
    public static class ObjectExtensions
    {
       
        public static void ExecuteRandom(this object self, IEnumerable<Action> methods)
        {
            methods?.GetRandom()?.Invoke();
        }

        public static bool IsType<T>(this object obj)
        {
            return obj != null && obj.GetType() == typeof(T);
        }

        public static T GetValue<T>(this object[] objects, int index)
        {
            try
            {
                return (T) objects[index];
            }
            catch (Exception)
            {
                return default;
            }
        }
        
        public static int GetObjectIdentifier(this object source)
        {
            return RuntimeHelpers.GetHashCode(source);
        }
    }
}