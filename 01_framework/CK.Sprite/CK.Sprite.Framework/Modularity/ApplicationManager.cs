using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public static class ApplicationManager
    {
        private static List<ISpriteModule> _spriteModules;
        static ApplicationManager()
        {
            _spriteModules = new List<ISpriteModule>();
            _spriteModules.Add(new FrameworkModule());
        }

        public static void AddModule(this IServiceCollection services, ISpriteModule spriteModule)
        {
            _spriteModules.Add(spriteModule);
        }

        public static void StartApp(this IServiceCollection services)
        {
            foreach(var spriteModule in _spriteModules)
            {
                spriteModule.Services = services;
                spriteModule.PreConfigureServices(services);
            }

            foreach (var spriteModule in _spriteModules)
            {
                spriteModule.PostConfigureServices(services);
            }
        }
    }
}
