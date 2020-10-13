using JetBrains.Annotations;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public interface IUnitOfWork : IDisposable
    {
        Guid Id { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void Begin(IsolationLevel il = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
    }
}
