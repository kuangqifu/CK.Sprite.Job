using JetBrains.Annotations;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CK.Sprite.Framework
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DbConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库连接类型
        /// </summary>
        public EConnectionType ConnectionType { get; set; }
    }

    public class DefaultDbConfig : DbConfig
    {
    }
}
