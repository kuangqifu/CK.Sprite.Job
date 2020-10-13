using CK.Sprite.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace System
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSpriteCore(this IApplicationBuilder app)
        {
            ServiceLocator.SetServices(app.ApplicationServices);
        }
    }

    public class ServiceLocator
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        internal static void SetServices(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
