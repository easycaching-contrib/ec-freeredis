namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : EasyCachingAbstractProvider
    {       
        public override Task<bool> BaseExistsAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }
     
        public override Task BaseFlushAsync()
        {
            throw new NotImplementedException();
        }
            
        public override Task<IDictionary<string, CacheValue<T>>> BaseGetAllAsync<T>(IEnumerable<string> cacheKeys)
        {
            throw new NotImplementedException();
        }

        public override Task<CacheValue<T>> BaseGetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public override Task<object> BaseGetAsync(string cacheKey, Type type)
        {
            throw new NotImplementedException();
        }

        public override Task<CacheValue<T>> BaseGetAsync<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }
     
        public override Task<IDictionary<string, CacheValue<T>>> BaseGetByPrefixAsync<T>(string prefix)
        {
            throw new NotImplementedException();
        }
       
        public override Task<int> BaseGetCountAsync(string prefix = "")
        {
            throw new NotImplementedException();
        }
      
        public override Task<TimeSpan> BaseGetExpirationAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }
       
        public override Task BaseRemoveAllAsync(IEnumerable<string> cacheKeys)
        {
            throw new NotImplementedException();
        }

        public override Task BaseRemoveAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }
     
        public override Task BaseRemoveByPrefixAsync(string prefix)
        {
            throw new NotImplementedException();
        }
             
        public override Task BaseSetAllAsync<T>(IDictionary<string, T> values, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public override Task BaseSetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }
       
        public override Task<bool> BaseTrySetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }
    }
}
