using JetBrains.Annotations;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public static class ServiceProviderDatabaseSessionExtensions
    {
        public static void DoDapperService(this IServiceProvider serviceProvider, DbConfig dbConfig, Action<IUnitOfWork> action)
        {
            using (DatabaseSession databaseSession = new DatabaseSession(dbConfig))
            {
                IUnitOfWork unitOfWork = databaseSession.UnitOfWork;
                unitOfWork.Begin();

                try
                {
                    action(unitOfWork);
                    unitOfWork.Commit();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public static TResult DoDapperService<TResult>(this IServiceProvider serviceProvider, DbConfig dbConfig, Func<IUnitOfWork, TResult> action)
        {
            using (DatabaseSession databaseSession = new DatabaseSession(dbConfig))
            {
                IUnitOfWork unitOfWork = databaseSession.UnitOfWork;
                unitOfWork.Begin();

                try
                {
                    var result = action(unitOfWork);
                    unitOfWork.Commit();
                    return result;
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public static async Task DoDapperServiceAsync(this IServiceProvider serviceProvider, DbConfig dbConfig, Func<IUnitOfWork, Task> action)
        {
            using (DatabaseSession databaseSession = new DatabaseSession(dbConfig))
            {
                IUnitOfWork unitOfWork = databaseSession.UnitOfWork;
                unitOfWork.Begin();

                try
                {
                    await action(unitOfWork);
                    unitOfWork.Commit();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public static async Task<TResult> DoDapperServiceAsync<TResult>(this IServiceProvider serviceProvider, DbConfig dbConfig, Func<IUnitOfWork, Task<TResult>> action)
        {
            using (DatabaseSession databaseSession = new DatabaseSession(dbConfig))
            {
                IUnitOfWork unitOfWork = databaseSession.UnitOfWork;
                unitOfWork.Begin();

                try
                {
                    var result = await action(unitOfWork);
                    unitOfWork.Commit();
                    return result;
                }
                catch(Exception ex)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }
    }
}
