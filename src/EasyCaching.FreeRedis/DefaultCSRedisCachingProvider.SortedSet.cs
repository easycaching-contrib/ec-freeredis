namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public long ZAdd<T>(string cacheKey, Dictionary<T, double> cacheValues)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZAddAsync<T>(string cacheKey, Dictionary<T, double> cacheValues)
        {
            throw new NotImplementedException();
        }

        public long ZCard(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZCardAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public long ZCount(string cacheKey, double min, double max)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZCountAsync(string cacheKey, double min, double max)
        {
            throw new NotImplementedException();
        }

        public double ZIncrBy(string cacheKey, string field, double val = 1)
        {
            throw new NotImplementedException();
        }

        public Task<double> ZIncrByAsync(string cacheKey, string field, double val = 1)
        {
            throw new NotImplementedException();
        }

        public long ZLexCount(string cacheKey, string min, string max)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZLexCountAsync(string cacheKey, string min, string max)
        {
            throw new NotImplementedException();
        }

        public List<T> ZRange<T>(string cacheKey, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> ZRangeAsync<T>(string cacheKey, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public List<T> ZRangeByScore<T>(string cacheKey, double min, double max, long? count = null, long offset = 0)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> ZRangeByScoreAsync<T>(string cacheKey, double min, double max, long? count = null, long offset = 0)
        {
            throw new NotImplementedException();
        }

        public long? ZRank<T>(string cacheKey, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public Task<long?> ZRankAsync<T>(string cacheKey, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public long ZRem<T>(string cacheKey, IList<T> cacheValues)
        {
            throw new NotImplementedException();
        }

        public Task<long> ZRemAsync<T>(string cacheKey, IList<T> cacheValues)
        {
            throw new NotImplementedException();
        }

        public double? ZScore<T>(string cacheKey, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public Task<double?> ZScoreAsync<T>(string cacheKey, T cacheValue)
        {
            throw new NotImplementedException();
        }
    }
}
