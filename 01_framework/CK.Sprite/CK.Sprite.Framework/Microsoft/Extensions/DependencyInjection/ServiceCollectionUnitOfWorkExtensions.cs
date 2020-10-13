using CK.Sprite.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionUnitOfWorkExtensions
    {
        public static void AddConnectionProvider(this IServiceCollection services, EConnectionType dbType, IConnectionProvider connectionProvider)
        {
            ConnectionFactory.AddConnectionProvider(dbType, connectionProvider);
        }

        public static void AddConnectionProvider(this IServiceCollection services, string dbKey, IConnectionProvider connectionProvider)
        {
            ConnectionFactory.AddConnectionProvider(dbKey, connectionProvider);
        }
    }
}
