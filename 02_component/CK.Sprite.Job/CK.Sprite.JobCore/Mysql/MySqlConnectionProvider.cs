using CK.Sprite.Framework;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace CK.Sprite.JobCore
{
    public class MySqlConnectionProvider : IConnectionProvider
    {
        public IDbConnection CreateDbConnection(string strConn)
        {
            return new MySqlConnection(strConn);
        }

        public T GetRepository<T>(IUnitOfWork unitOfWork) where T:class, IRepository
        {
            if (typeof(T).IsAssignableFrom(typeof(IJobConfigRepository)))
            {
                return new JobConfigRepository(unitOfWork) as T;
            }

            if (typeof(T).IsAssignableFrom(typeof(IHolidayCalendarRepository)))
            {
                return new HolidayCalendarRepository(unitOfWork) as T;
            }

            return default(T);
        }
    }
}
