using AutoMapper;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public class FrameworkModule : SpriteModule
    {
        public override void PreConfigureServices(IServiceCollection Services)
        {
            Services.AddAssemblyOf<FrameworkModule>();
        }
    }
}
