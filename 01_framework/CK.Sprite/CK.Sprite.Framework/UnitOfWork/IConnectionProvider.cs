using JetBrains.Annotations;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public interface IConnectionProvider
    {
        /// <summary>
        /// 创建DbConnection
        /// </summary>
        /// <param name="strConn">连接字符串</param>
        /// <returns></returns>
        IDbConnection CreateDbConnection(string strConn);

        /// <summary>
        /// 获取仓储信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        T GetRepository<T>(IUnitOfWork unitOfWork) where T : class, IRepository;
    }
}
