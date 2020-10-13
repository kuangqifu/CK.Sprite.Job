using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CK.Sprite.Framework
{
    public static class JTokenHelper
    {
        public static object ToConventionalDotNetObject(this JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return ((JObject)token).Properties()
                        .ToDictionary(prop => prop.Name, prop => prop.Value.ToConventionalDotNetObject());
                case JTokenType.Array:
                    return token.Values().Select(ToConventionalDotNetObject).ToList();
                default:
                    return token.ToObject<object>();
            }
        }
    }
}
