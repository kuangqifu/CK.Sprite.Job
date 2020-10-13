using JetBrains.Annotations;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    /// <summary>
    /// 当前租户信息
    /// </summary>
    public class DefaultCurrentUser : ICurrentUser, ITransientDependency
    {
        public string UserId => "adminId";

        public string UserName => "admin";
    }
}
