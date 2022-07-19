namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public string RedisName => throw new NotImplementedException();

        public object Eval(string script, string cacheKey, List<object> args)
        {
            throw new NotImplementedException();
        }

        public Task<object> EvalAsync(string script, string cacheKey, List<object> args)
        {
            throw new NotImplementedException();
        }

        public bool KeyDel(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<bool> KeyDelAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public bool KeyExists(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<bool> KeyExistsAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public bool KeyExpire(string cacheKey, int second)
        {
            throw new NotImplementedException();
        }

        public Task<bool> KeyExpireAsync(string cacheKey, int second)
        {
            throw new NotImplementedException();
        }

        public long TTL(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<long> TTLAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }
    }
}
