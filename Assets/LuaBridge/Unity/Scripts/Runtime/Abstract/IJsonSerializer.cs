using System;

namespace LuaBridge.Core.Abstract
{
    public interface IJsonSerializer
    {
        public string ToJson(object obj);
        public string ToStronglyTypedJson(object obj);
        public T FromJson<T>(string json);
        public object FromJson(string json, Type type);
    }
}