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

        public Task<long> IncrByAsync(string cacheKey, long value = 1)
        {
            throw new NotImplementedException();
        }

        public double IncrByFloat(string cacheKey, double value = 1)
        {
            throw new NotImplementedException();
        }

        public Task<double> IncrByFloatAsync(string cacheKey, double value = 1)
        {
            throw new NotImplementedException();
        }

        public string StringGet(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<string> StringGetAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public string StringGetRange(string cacheKey, long start, long end)
        {
            throw new NotImplementedException();
        }

        public Task<string> StringGetRangeAsync(string cacheKey, long start, long end)
        {
            throw new NotImplementedException();
        }

        public long StringLen(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<long> StringLenAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public bool StringSet(string cacheKey, string cacheValue, TimeSpan? expiration = null, string when = "")
        {
            throw new NotImplementedException();
        }

        public Task<bool> StringSetAsync(string cacheKey, string cacheValue, TimeSpan? expiration = null, string when = "")
        {
            throw new NotImplementedException();
        }

        public long StringSetRange(string cacheKey, long offest, string value)
        {
            throw new NotImplementedException();
        }

        public Task<long> StringSetRangeAsync(string cacheKey, long offest, string value)
        {
            throw new NotImplementedException();
        }
    }
}
