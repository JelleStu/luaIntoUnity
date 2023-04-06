using System;
using LuaBridge.Core.Abstract;
using Newtonsoft.Json;

namespace LuaBridge.Unity.Scripts.LuaBridgeHelpers.JSonSerializer
{
    public class JsonSerializer : IJsonSerializer
    {
        public string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);        
        }

        public string ToStronglyTypedJson(object obj)
        {
            throw new NotImplementedException();
        }

        public T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
            
        }

        public object FromJson(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
            
        }
    }
}