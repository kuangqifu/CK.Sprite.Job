using AutoMapper;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    /// <summary>
    /// 本地缓存接口
    /// </summary>
    /// <typeparam name="T">缓存实体</typeparam>
    public interface ILocalCache<T> : ISingletonDependency
    {
        List<T> GetAll(string applicationCode);
    }

    public abstract class LocalCache<T> : ILocalCache<T>
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

        protected DefaultDbConfig DefaultDbConfig => LazyGetRequiredService(ref _defaultDbConfig).Value;
        private IOptions<DefaultDbConfig> _defaultDbConfig;
        
        protected ISerializeService SerializeService => LazyGetRequiredService(ref _serializeService);
        private ISerializeService _serializeService;

        public abstract List<T> GetAll(string applicationCode);

        public abstract string CacheKey { get; }
    }
}
