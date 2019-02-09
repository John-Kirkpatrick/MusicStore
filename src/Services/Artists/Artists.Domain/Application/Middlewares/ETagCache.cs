#region references

using System;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

#endregion

namespace Artists.Domain.Application.Middlewares {
    public interface IETagCache {
        #region Public Methods

        T GetCachedObject<T>(
            string cacheKeyPrefix
        );

        void RemoveCachedObject(
            string cacheKeyPrefix
        );

        void SetCachedObject(
            string cacheKeyPrefix,
            dynamic objectToCache,
            DistributedCacheEntryOptions cacheOptions = null
        );

        #endregion
    }

    /// <summary>
    ///     https://www.carlrippon.com/scalable-and-performant-asp-net-core-web-apis-server-caching/
    ///     https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/ETag
    /// </summary>
    public class ETagCache : IETagCache {
        #region Private Fields

        private readonly IDistributedCache _cache;

        #endregion

        #region Public Constructors

        public ETagCache(
            IDistributedCache cache
        ) {
            _cache = cache;
        }

        #endregion

        #region Public Methods

        public T GetCachedObject<T>(
            string cacheKeyPrefix
        ) {
            // Get the cached item
            string cachedObjectJson = _cache.GetString(cacheKeyPrefix);

            // If there was a cached item then deserialise this 
            if (!string.IsNullOrEmpty(cachedObjectJson)) {
                T cachedObject = JsonConvert.DeserializeObject<T>(cachedObjectJson);
                return cachedObject;
            }

            return default(T);
        }

        public void RemoveCachedObject(
            string cacheKeyPrefix
        ) {
            _cache.Remove(cacheKeyPrefix);
        }

        public void SetCachedObject(
            string cacheKeyPrefix,
            dynamic objectToCache,
            DistributedCacheEntryOptions cacheOptions = null
        ) {
            // Add the object to the cache for 30 mins if not already in the cache
            if (objectToCache != null) {
                string serializedObjectToCache = JsonConvert.SerializeObject(objectToCache);
                _cache.SetString(
                    cacheKeyPrefix,
                    serializedObjectToCache,
                    cacheOptions ??
                    new DistributedCacheEntryOptions {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(30)
                    }
                );
            }
        }

        #endregion
    }
}