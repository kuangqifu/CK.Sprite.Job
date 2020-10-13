using AutoMapper;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public abstract class AppService : IAppService, ITransientDependency
    {
        protected IServiceProvider _serviceProvider => ServiceLocator.ServiceProvider;
        protected readonly object ServiceProviderLock = new object();
        public AppService()
        {
        }

        protected TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)_serviceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        public IMapper Mapper => LazyGetRequiredService(ref _mapper);
        private IMapper _mapper;
    }
}
