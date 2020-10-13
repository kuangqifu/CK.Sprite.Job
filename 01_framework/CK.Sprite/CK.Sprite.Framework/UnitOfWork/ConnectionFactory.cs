using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    public class ConnectionFactory
    {
        private static Dictionary<EConnectionType, IConnectionProvider> _connectionProviders;
        private static Dictionary<string, IConnectionProvider> _connectionProviderStrings;

        static ConnectionFactory()
        {
            _connectionProviders = new Dictionary<EConnectionType, IConnectionProvider>();
            _connectionProviderStrings = new Dictionary<string, IConnectionProvider>();
        }

        internal static void AddConnectionProvider(EConnectionType dbType, IConnectionProvider connectionProvider)
        {
            if(!_connectionProviders.ContainsKey(dbType))
            {
                _connectionProviders.Add(dbType, connectionProvider);
            }
        }

        internal static void AddConnectionProvider(string dbKey, IConnectionProvider connectionProvider)
        {
            if (!_connectionProviderStrings.ContainsKey(dbKey))
            {
                _connectionProviderStrings.Add(dbKey, connectionProvider);
            }
        }

        public static IConnectionProvider GetConnectionProvider(EConnectionType dbType)
        {
            return _connectionProviders[dbType];
        }

        public static IConnectionProvider GetConnectionProvider(string dbKey)
        {
            return _connectionProviderStrings[dbKey];
        }
    }
}
