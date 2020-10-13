using JetBrains.Annotations;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public sealed class DatabaseSession : IDisposable
    {
        IDbConnection _connection = null;
        IUnitOfWork _unitOfWork = null;

        public DatabaseSession(DbConfig dbConfig)
        {
            try
            {
                _connection = ConnectionFactory.GetConnectionProvider(dbConfig.ConnectionType).CreateDbConnection(dbConfig.ConnectionString);
                _connection.Open();
                _unitOfWork = new UnitOfWork(_connection);
            }
            catch(Exception ex)
            {
                throw new SpriteException($"数据库连接失败，{ex.Message}，请联系管理员");
            }
        }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _connection.Dispose();
        }
    }
}
