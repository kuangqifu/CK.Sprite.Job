using JetBrains.Annotations;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    /// <summary>
    /// 当前用户信息
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// 用户名称
        /// </summary>
        string UserName { get; }
    }
}
