using JetBrains.Annotations;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CK.Sprite.Framework
{
    /// <summary>
    /// 租户配置
    /// </summary>
    public class TenantConfig : DbConfig
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 应用Code
        /// </summary>
        public string ApplicationCode { get; set; }

        /// <summary>
        /// 租户Code
        /// </summary>
        public string TenantCode { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// 是否单数据库存储
        /// </summary>
        public bool IsSingleDbStore { get; set; }
    }
}
