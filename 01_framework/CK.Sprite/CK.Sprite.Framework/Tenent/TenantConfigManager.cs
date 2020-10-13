using JetBrains.Annotations;
using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public class TenantConfigManager : ISingletonDependency
    {
        private ConcurrentDictionary<string, TenantConfig> _cacheDict;
        private ITenantConfigStore _tenantConfigStore;

        public TenantConfigManager(ITenantConfigStore tenantConfigStore)
        {
            _cacheDict = new ConcurrentDictionary<string, TenantConfig>();
            _tenantConfigStore = tenantConfigStore;
        }

        /// <summary>
        /// 获取租户配置信息，如果缓存里面没有，从数据库获取
        /// </summary>
        /// <param name="applicationCode">应用Code</param>
        /// <param name="tenantCode">租户Code</param>
        /// <returns></returns>
        public TenantConfig GetTenantConfig(string applicationCode, string tenantCode)
        {
            var key = $"{applicationCode}_{tenantCode}";
            return _cacheDict.GetOrAdd(key, (key1) =>
            {
                var result = _tenantConfigStore.Find(applicationCode, tenantCode);
                if (result == null)
                {
                    throw new SpriteException("租户配置信息未找到");
                }
                return result;
            });
        }
    }
}
