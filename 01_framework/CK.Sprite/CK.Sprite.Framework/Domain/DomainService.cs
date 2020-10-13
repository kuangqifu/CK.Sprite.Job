using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public abstract class DomainService : IDomainService
    {
        protected IServiceProvider _serviceProvider => ServiceLocator.ServiceProvider;
        protected readonly object ServiceProviderLock = new object();

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

        public DefaultDbConfig DefaultDbConfig => LazyGetRequiredService(ref _defaultDbConfig).Value;
        private IOptions<DefaultDbConfig> _defaultDbConfig;

        public ILogger<DomainService> Logger => LazyGetRequiredService(ref _logger);
        private ILogger<DomainService> _logger;

        protected ITenantConfigStore TenantConfigStore => LazyGetRequiredService(ref _tenantConfigStore);
        private ITenantConfigStore _tenantConfigStore;
    }
}
