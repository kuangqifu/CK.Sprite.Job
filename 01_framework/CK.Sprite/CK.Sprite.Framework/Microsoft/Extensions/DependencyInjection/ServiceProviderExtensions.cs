using CK.Sprite.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }

        public static T GetService<T>(this IServiceProvider serviceProvider, string reflectInfo)
        {
            return (T)serviceProvider.GetService(Type.GetType(reflectInfo));
        }
    }
}
