namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public bool HMSet(string cacheKey, Dictionary<string, string> vals, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }

        public bool HSet(string cacheKey, string field, string cacheValue)
        {
            throw new NotImplementedException();
        }

        public bool HExists(string cacheKey, string field)
        {
            throw new NotImplementedException();
        }

        public long HDel(string cacheKey, IList<string> fields = null)
        {
            throw new NotImplementedException();
        }

        public string HGet(string cacheKey, string field)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> HGetAll(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public long HIncrBy(string cacheKey, string field, long val = 1)
        {
            throw new NotImplementedException();
        }

        public List<string> HKeys(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public long HLen(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public List<string> HVals(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> HMGet(string cacheKey, IList<string> fields)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> HMSetAsync(string cacheKey, Dictionary<string, string> vals, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> HSetAsync(string cacheKey, string field, string cacheValue)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> HExistsAsync(string cacheKey, string field)
        {
            throw new NotImplementedException();
        }

        public async Task<long> HDelAsync(string cacheKey, IList<string> fields)
        {
            throw new NotImplementedException();
        }

        public async Task<string> HGetAsync(string cacheKey, string field)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, string>> HGetAllAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public async Task<long> HIncrByAsync(string cacheKey, string field, long val = 1)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> HKeysAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public async Task<long> HLenAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> HValsAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, string>> HMGetAsync(string cacheKey, IList<string> fields)
        {
            throw new NotImplementedException();
        }
    }
}
