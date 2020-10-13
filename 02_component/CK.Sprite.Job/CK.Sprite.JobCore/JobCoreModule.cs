using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using CK.Sprite.Framework;

namespace CK.Sprite.JobCore
{
    public class JobCoreModule : SpriteModule
    {
        public override void PreConfigureServices(IServiceCollection Services)
        {
            Services.AddAssemblyOf<JobCoreModule>();

            Services.AddAutoMapper(typeof(AutomapperConfig));

            Services.AddConnectionProvider(EConnectionType.MySql, new MySqlConnectionProvider());
        }
    }
}
