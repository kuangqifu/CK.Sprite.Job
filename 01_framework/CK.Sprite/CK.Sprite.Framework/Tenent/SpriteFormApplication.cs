using JetBrains.Annotations;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CK.Sprite.Framework
{
    /// <summary>
    /// 表单应用分类
    /// </summary>
    public class SpriteFormApplication
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 应用Code，唯一，业务系统都使用此字段
        /// </summary>
        public string ApplicationCode { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; }
    }
}
