using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CK.Sprite.Framework
{
    public interface ISerializeService
    {
        string SerializeObject(object obj);
        T DeserializeObject<T>(string value);
    }

    public class DefaultSerializeService : ISerializeService, ISingletonDependency
    {
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
