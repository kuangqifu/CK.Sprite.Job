using JetBrains.Annotations;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public class UnitOfWork : IUnitOfWork
    {
        internal UnitOfWork(IDbConnection connection)
        {
            _id = Guid.NewGuid();
            _connection = connection;
        }

        IDbConnection _connection = null;
        IDbTransaction _transaction = null;
        Guid _id = Guid.Empty;

        public IDbConnection Connection
        {
            get { return _connection; }
        }
        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }
        public Guid Id
        {
            get { return _id; }
        }

        public void Begin(IsolationLevel il = IsolationLevel.ReadCommitted)
        {
            _transaction = _connection.BeginTransaction(il);
        }

        public void Commit()
        {
            _transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
            _transaction = null;
        }
    }
}
