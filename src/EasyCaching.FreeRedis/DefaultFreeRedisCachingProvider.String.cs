namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using System;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public long IncrBy(string cacheKey, long value = 1)
        {
            throw new NotImplementedException();
        }

        public async Task<long> IncrByAsync(string cacheKey, long value = 1)
        {
            throw new NotImplementedException();
        }

        public double IncrByFloat(string cacheKey, double value = 1)
        {
            throw new NotImplementedException();
        }

        public async Task<double> IncrByFloatAsync(string cacheKey, double value = 1)
        {
            throw new NotImplementedException();
        }

        public bool StringSet(string cacheKey, string cacheValue, System.TimeSpan? expiration, string when)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> StringSetAsync(string cacheKey, string cacheValue, System.TimeSpan? expiration, string when)
        {
            throw new NotImplementedException();
        }

        public string StringGet(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public async Task<string> StringGetAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public long StringLen(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public async Task<long> StringLenAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public long StringSetRange(string cacheKey, long offest, string value)
        {
            throw new NotImplementedException();
        }

        public async Task<long> StringSetRangeAsync(string cacheKey, long offest, string value)
        {
            throw new NotImplementedException();
        }

        public string StringGetRange(string cacheKey, long start, long end)
        {
            throw new NotImplementedException();
        }

        public async Task<string> StringGetRangeAsync(string cacheKey, long start, long end)
        {
            throw new NotImplementedException();
        }
    }
}
