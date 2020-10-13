using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public interface ISpriteModule
    {
        IServiceCollection Services { get; set; }
        void PreConfigureServices(IServiceCollection Services);

        void PostConfigureServices(IServiceCollection Services);
    }

    public abstract class SpriteModule : ISpriteModule
    {
        public IServiceCollection Services { get; set; }

        public SpriteModule()
        {
        }

        public virtual void PostConfigureServices(IServiceCollection Services)
        {            
        }

        public virtual void PreConfigureServices(IServiceCollection Services)
        {
        }

        protected void Configure<TOptions>(Action<TOptions> configureOptions)
            where TOptions : class
        {
            Services.Configure(configureOptions);
        }

        protected void Configure<TOptions>(string name, Action<TOptions> configureOptions)
            where TOptions : class
        {
            Services.Configure(name, configureOptions);
        }

        protected void Configure<TOptions>(IConfiguration configuration)
            where TOptions : class
        {
            Services.Configure<TOptions>(configuration);
        }

        protected void Configure<TOptions>(IConfiguration configuration, Action<BinderOptions> configureBinder)
            where TOptions : class
        {
            Services.Configure<TOptions>(configuration, configureBinder);
        }

        protected void Configure<TOptions>(string name, IConfiguration configuration)
            where TOptions : class
        {
            Services.Configure<TOptions>(name, configuration);
        }
    }
}
