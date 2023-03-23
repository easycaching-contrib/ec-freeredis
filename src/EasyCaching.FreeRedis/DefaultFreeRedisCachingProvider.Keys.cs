namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public string RedisName => this._name;

        public bool KeyDel(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> KeyDelAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public bool KeyExpire(string cacheKey, int second)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> KeyExpireAsync(string cacheKey, int second)
        {
            throw new NotImplementedException();
        }

        public bool KeyPersist(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> KeyPersistAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public bool KeyExists(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> KeyExistsAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public long TTL(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public async Task<long> TTLAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public object Eval(string script, string cacheKey, List<object> args)
        {
            throw new NotImplementedException();
        }

        public async Task<object> EvalAsync(string script, string cacheKey, List<object> args)
        {
            throw new NotImplementedException();
        }

        public List<string> SearchKeys(string cacheKey, int? count)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> SearchKeysAsync(string cacheKey, int? count)
        {
            throw new NotImplementedException();
        }
    }
}
