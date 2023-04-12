namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using global::FreeRedis;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public long ZAdd<T>(string cacheKey, Dictionary<T, double> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var param = new List<ZMember>();

            foreach (var item in cacheValues)
            {
                param.Add(new ZMember(ConvertTo<string>(item.Key), (decimal)item.Value));
            }

            var len = _cache.ZAdd(cacheKey, param.ToArray());

            return len;
        }

        public long ZCard(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = _cache.ZCard(cacheKey);
            return len;
        }

        public long ZCount(string cacheKey, double min, double max)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = _cache.ZCount(cacheKey, (decimal)min, (decimal)max);
            return len;
        }
        public double ZIncrBy(string cacheKey, string field, double val = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(field, nameof(field));

            var value = _cache.ZIncrBy(cacheKey, (decimal)val, field);
            return (double)value;
        }
        public long ZLexCount(string cacheKey, string min, string max)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = _cache.ZLexCount(cacheKey, min, max);
            return len;
        }

        public List<T> ZRange<T>(string cacheKey, long start, long stop)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var member = _cache.ZRange(cacheKey, start, stop);

            foreach (var item in member)
            {
                list.Add(ConvertTo<T>(item));
            }

            return list;
        }

        public List<T> ZRangeByScore<T>(string cacheKey, double min, double max, long? count = null, long offset = 0)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var members = _cache.ZRangeByScore(cacheKey, (decimal)min, (decimal)max, (int)offset, (int)(count ?? 0));

            foreach (var item in members)
            {
                list.Add(ConvertTo<T>(item));
            }

            return list;
        }

        public long ZRangeRemByScore(string cacheKey, double min, double max)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return _cache.ZRemRangeByScore(cacheKey, (decimal)min, (decimal)max);
        }

        public long? ZRank<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var index = _cache.ZRank(cacheKey, ConvertTo<string>(cacheValue));

            return index;
        }


        public long ZRem<T>(string cacheKey, IList<T> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var members = new List<string>();

            foreach (var item in cacheValues)
            {
                members.Add(ConvertTo<string>(item));
            }

            var len = _cache.ZRem(cacheKey, members.ToArray());

            return len;
        }

        public double? ZScore<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var member = ConvertTo<string>(cacheValue);

            var score = _cache.ZScore(cacheKey, member);

            return (double?)score;
        }

        public async Task<long> ZAddAsync<T>(string cacheKey, Dictionary<T, double> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var param = new List<ZMember>();

            foreach (var item in cacheValues)
            {
                param.Add(new ZMember(ConvertTo<string>(item.Key), (decimal)item.Value));
            }

            var len = await _cache.ZAddAsync(cacheKey, param.ToArray());

            return len;
        }


        public async Task<long> ZCardAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = await _cache.ZCardAsync(cacheKey);
            return len;
        }

        public async Task<long> ZCountAsync(string cacheKey, double min, double max)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = await _cache.ZCountAsync(cacheKey, (decimal)min, (decimal)max);
            return len;
        }

        public async Task<double> ZIncrByAsync(string cacheKey, string field, double val = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(field, nameof(field));

            var value = await _cache.ZIncrByAsync(cacheKey, (decimal)val, field);
            return (double)value;
        }

        public async Task<long> ZLexCountAsync(string cacheKey, string min, string max)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = await _cache.ZLexCountAsync(cacheKey, min, max);
            return len;
        }

        public async Task<List<T>> ZRangeAsync<T>(string cacheKey, long start, long stop)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var members = await _cache.ZRangeAsync(cacheKey, start, stop);

            foreach (var item in members)
            {
                list.Add(ConvertTo<T>(item));
            }

            return list;
        }

        public async Task<List<T>> ZRangeByScoreAsync<T>(string cacheKey, double min, double max, long? count = null, long offset = 0)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var members = await _cache.ZRangeByScoreAsync(cacheKey, (decimal)min, (decimal)max, (int)offset, (int)(count ?? 0));

            foreach (var item in members)
            {
                list.Add(ConvertTo<T>(item));
            }

            return list;
        }

        public async Task<long> ZRangeRemByScoreAsync(string cacheKey, double min, double max)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return await _cache.ZRemRangeByScoreAsync(cacheKey, (decimal)min, (decimal)max);
        }

        public async Task<long?> ZRankAsync<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var member = ConvertTo<string>(cacheValue);

            var index = await _cache.ZRankAsync(cacheKey, member);

            return index;
        }

        public async Task<long> ZRemAsync<T>(string cacheKey, IList<T> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var members = new List<string>();

            foreach (var item in cacheValues)
            {
                members.Add(ConvertTo<string>(item));
            }

            var len = await _cache.ZRemAsync(cacheKey, members.ToArray());

            return len;
        }

        public async Task<double?> ZScoreAsync<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var member = ConvertTo<string>(cacheValue);

            var score = await _cache.ZScoreAsync(cacheKey, member);

            return (double?)score;
        }

        /// <summary>
        /// Convert to T for FreeRedis RespHelper Method
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="value">object value</param>
        /// <returns></returns>
        T ConvertTo<T>(object value) => (T)typeof(T).FromObject(value);
    }

}
