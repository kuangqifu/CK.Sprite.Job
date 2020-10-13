using JetBrains.Annotations;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public class DefaultTenantStoreOptions
    {
        public TenantConfig[] Tenants { get; set; }

        public DefaultTenantStoreOptions()
        {
            Tenants = new TenantConfig[0];
        }
    }
}
