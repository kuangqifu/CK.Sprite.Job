using JetBrains.Annotations;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }

    public class GuidEntityBase : IEntity<Guid>
    {
        [Dapper.Contrib.Extensions.ExplicitKey]
        public Guid Id { get; set; }
    }
}
