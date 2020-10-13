using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CK.Sprite.Framework;
using CK.Sprite.JobCore;

namespace CK.Sprite.JobWebHost
{
    public class WebHostModule : SpriteModule
    {
        public override void PreConfigureServices(IServiceCollection Services)
        {
            Services.AddAssemblyOf<WebHostModule>();

            Configure<MvcOptions>(mvcOptions =>
            {
                mvcOptions.AddException(Services);
            });

            var configuration = Services.GetConfiguration(); 
            Configure<DefaultDbConfig>(configuration.GetSection("DefaultDbConfig"));
            Configure<CallApiConfig>(configuration.GetSection("CallApiConfig"));
        }
    }
}
