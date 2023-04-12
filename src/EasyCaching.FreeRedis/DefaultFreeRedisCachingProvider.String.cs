namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using System;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public long IncrBy(string cacheKey, long value = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = _cache.IncrBy(cacheKey, value);
            return res;
        }

        public async Task<long> IncrByAsync(string cacheKey, long value = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = await _cache.IncrByAsync(cacheKey, value);
            return res;
        }

        public double IncrByFloat(string cacheKey, double value = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = _cache.IncrByFloat(cacheKey, (decimal)value);
            return (double)res;
        }
        public async Task<double> IncrByFloatAsync(string cacheKey, double value = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = await _cache.IncrByFloatAsync(cacheKey, (decimal)value);
            return (double)res;
        }

        public bool StringSet(string cacheKey, string cacheValue, System.TimeSpan? expiration, string when)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            bool isNx = when.Equals("nx", System.StringComparison.OrdinalIgnoreCase);
            bool isXx = when.Equals("xx", System.StringComparison.OrdinalIgnoreCase);

            var flag = false;
            if (expiration.HasValue)
            {
                flag = _cache.Set(cacheKey, cacheValue, expiration.Value, false, isNx, isXx, false) == "OK";
            }
            else
            {
                flag = _cache.Set(cacheKey, cacheValue, TimeSpan.FromSeconds(0), false, isNx, isXx, false) == "OK";
            }

            return flag;
        }

        public async Task<bool> StringSetAsync(string cacheKey, string cacheValue, System.TimeSpan? expiration, string when)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            bool isNx = when.Equals("nx", System.StringComparison.OrdinalIgnoreCase);
            bool isXx = when.Equals("xx", System.StringComparison.OrdinalIgnoreCase);

            var flag = false;
            if (expiration.HasValue)
            {
                flag = await _cache.SetAsync(cacheKey, cacheValue, expiration.Value, false, isNx, isXx, false) == "OK";
            }
            else
            {
                flag = await _cache.SetAsync(cacheKey, cacheValue, TimeSpan.FromSeconds(0), false, isNx, isXx, false) == "OK";
            }

            return flag;
        }

        public string StringGet(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var val = _cache.Get(cacheKey);
            return val;
        }

        public async Task<string> StringGetAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var val = await _cache.GetAsync(cacheKey);
            return val;
        }

        public long StringLen(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = _cache.StrLen(cacheKey);
            return len;
        }

        public async Task<long> StringLenAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = await _cache.StrLenAsync(cacheKey);
            return len;
        }


        public long StringSetRange(string cacheKey, long offest, string value)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = _cache.SetRange(cacheKey, (uint)offest, value);
            return res;
        }

        public async Task<long> StringSetRangeAsync(string cacheKey, long offest, string value)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = await _cache.SetRangeAsync(cacheKey, (uint)offest, value);
            return res;
        }

        public string StringGetRange(string cacheKey, long start, long end)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = _cache.GetRange(cacheKey, start, end);
            return res;
        }

        public async Task<string> StringGetRangeAsync(string cacheKey, long start, long end)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = await _cache.GetRangeAsync(cacheKey, start, end);
            return res;
        }
    }
}
