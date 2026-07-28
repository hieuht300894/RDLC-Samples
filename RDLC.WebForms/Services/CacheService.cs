using System;
using System.Runtime.Caching;

namespace RDLC.WebForms.Services
{
    public class CacheService
    {
        private readonly ObjectCache _cache = MemoryCache.Default;

        public object GetData(string key)
        {
            var cachedItem = _cache.Get(key);
            if (cachedItem != null)
            {
                return cachedItem;
            }

            return null;
        }

        public void SetData(string key, object value, int timeoutInSeconds = 60)
        {
            _cache.Set(key, value, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(timeoutInSeconds),
            });
        }
    }
}