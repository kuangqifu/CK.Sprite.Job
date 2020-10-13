using JetBrains.Annotations;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CK.Sprite.Framework
{
    /// <summary>
    /// 本地缓存容器
    /// </summary>
    public class LocalCacheContainer
    {
        private static ConcurrentDictionary<string, object> CacheDict;
        private static ConcurrentDictionary<string, ReaderWriterLock> CacheReaderWriterLockDict;

        static LocalCacheContainer()
        {
            CacheDict = new ConcurrentDictionary<string, object>();
            CacheReaderWriterLockDict = new ConcurrentDictionary<string, ReaderWriterLock>();
        }

        public static bool IsEnabledLocalCache { get; private set; } = true;

        /// <summary>
        /// 缓存通知断开时调用
        /// </summary>
        /// <param name="isEnabled">是否启用缓存</param>
        public static void SetLocalCacheIsEnabled(bool isEnabled)
        {
            IsEnabledLocalCache = isEnabled;
            if(!isEnabled)
            {
                ClearAllCache();
            }
        }

        public static object Get(string key, Func<string, object> factory)
        {
            var readerWriterLock = GetReadWriteLock(key);
            readerWriterLock.AcquireReaderLock(5000);

            try
            {
                return CacheDict.GetOrAdd(key, factory);
            }
            finally
            {
                readerWriterLock.ReleaseReaderLock();
            }
        }

        public static void ClearCache(string key)
        {
            var readerWriterLock = GetReadWriteLock(key);
            readerWriterLock.AcquireWriterLock(5000);

            try
            {
                object objRemove;
                CacheDict.TryRemove(key, out objRemove);
            }
            finally
            {
                readerWriterLock.ReleaseReaderLock();
            }
        }

        // 清楚所有缓存信息
        private static void ClearAllCache()
        {
            CacheDict.Clear();
            CacheReaderWriterLockDict.Clear();
        }

        private static ReaderWriterLock GetReadWriteLock(string key)
        {
            return CacheReaderWriterLockDict.GetOrAdd(key, k =>
            {
                return new ReaderWriterLock();
            });
        }
    }
}
