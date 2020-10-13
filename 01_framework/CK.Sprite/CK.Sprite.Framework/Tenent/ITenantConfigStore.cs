using JetBrains.Annotations;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public interface ITenantConfigStore
    {
        Task<TenantConfig> FindAsync(string applicationCode, string tennentCode);

        TenantConfig Find(string applicationCode, string tennentCode);
    }
}
