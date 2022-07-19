namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public T LIndex<T>(string cacheKey, long index)
        {
            throw new NotImplementedException();
        }

        public Task<T> LIndexAsync<T>(string cacheKey, long index)
        {
            throw new NotImplementedException();
        }

        public long LInsertAfter<T>(string cacheKey, T pivot, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public Task<long> LInsertAfterAsync<T>(string cacheKey, T pivot, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public long LInsertBefore<T>(string cacheKey, T pivot, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public Task<long> LInsertBeforeAsync<T>(string cacheKey, T pivot, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public long LLen(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<long> LLenAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public T LPop<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<T> LPopAsync<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public long LPush<T>(string cacheKey, IList<T> cacheValues)
        {
            throw new NotImplementedException();
        }

        public Task<long> LPushAsync<T>(string cacheKey, IList<T> cacheValues)
        {
            throw new NotImplementedException();
        }

        public long LPushX<T>(string cacheKey, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public Task<long> LPushXAsync<T>(string cacheKey, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public List<T> LRange<T>(string cacheKey, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> LRangeAsync<T>(string cacheKey, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public long LRem<T>(string cacheKey, long count, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public Task<long> LRemAsync<T>(string cacheKey, long count, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public bool LSet<T>(string cacheKey, long index, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LSetAsync<T>(string cacheKey, long index, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public bool LTrim(string cacheKey, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LTrimAsync(string cacheKey, long start, long stop)
        {
            throw new NotImplementedException();
        }

        public T RPop<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<T> RPopAsync<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public long RPush<T>(string cacheKey, IList<T> cacheValues)
        {
            throw new NotImplementedException();
        }

        public Task<long> RPushAsync<T>(string cacheKey, IList<T> cacheValues)
        {
            throw new NotImplementedException();
        }

        public long RPushX<T>(string cacheKey, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public Task<long> RPushXAsync<T>(string cacheKey, T cacheValue)
        {
            throw new NotImplementedException();
        }
    }
}
