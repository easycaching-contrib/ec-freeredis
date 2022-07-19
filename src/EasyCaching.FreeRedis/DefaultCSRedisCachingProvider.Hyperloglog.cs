namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public bool PfAdd<T>(string cacheKey, List<T> values)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PfAddAsync<T>(string cacheKey, List<T> values)
        {
            throw new NotImplementedException();
        }

        public long PfCount(List<string> cacheKeys)
        {
            throw new NotImplementedException();
        }

        public Task<long> PfCountAsync(List<string> cacheKeys)
        {
            throw new NotImplementedException();
        }

        public bool PfMerge(string destKey, List<string> sourceKeys)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PfMergeAsync(string destKey, List<string> sourceKeys)
        {
            throw new NotImplementedException();
        }
    }
}
