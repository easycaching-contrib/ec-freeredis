namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public long SAdd<T>(string cacheKey, IList<T> cacheValues, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> SAddAsync<T>(string cacheKey, IList<T> cacheValues, TimeSpan? expiration = null)
        {
            throw new NotImplementedException();
        }

        public long SCard(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<long> SCardAsync(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public List<string> SearchKeys(string cacheKey, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> SearchKeysAsync(string cacheKey, int? count = null)
        {
            throw new NotImplementedException();
        }

        public bool SIsMember<T>(string cacheKey, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SIsMemberAsync<T>(string cacheKey, T cacheValue)
        {
            throw new NotImplementedException();
        }

        public List<T> SMembers<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> SMembersAsync<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public T SPop<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public Task<T> SPopAsync<T>(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public List<T> SRandMember<T>(string cacheKey, int count = 1)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> SRandMemberAsync<T>(string cacheKey, int count = 1)
        {
            throw new NotImplementedException();
        }

        public long SRem<T>(string cacheKey, IList<T> cacheValues = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> SRemAsync<T>(string cacheKey, IList<T> cacheValues = null)
        {
            throw new NotImplementedException();
        }
    }
}
