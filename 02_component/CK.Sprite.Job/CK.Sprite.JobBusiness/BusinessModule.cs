using CK.Sprite.Framework;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CK.Sprite.JobBusiness
{
    public class BusinessModule : SpriteModule
    {
        public override void PreConfigureServices(IServiceCollection Services)
        {
            Services.AddAssemblyOf<BusinessModule>();
        }
    }
}
