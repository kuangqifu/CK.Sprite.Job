using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public class DefaultTenantConfigStore : ITenantConfigStore, ITransientDependency
    {
        // 直接从 Options 当中获取租户配置数据。
        private readonly DefaultTenantStoreOptions _options;

        public DefaultTenantConfigStore(IOptions<DefaultTenantStoreOptions> options)
        {
            _options = options.Value;
        }

        public Task<TenantConfig> FindAsync(string applicationCode, string tennentCode)
        {
            return Task.FromResult(Find(applicationCode, tennentCode));
        }

        public TenantConfig Find(string applicationCode, string tennentCode)
        {
            return _options.Tenants?.FirstOrDefault(t => t.ApplicationCode == applicationCode && t.TenantCode == tennentCode);
        }
    }
}
