﻿namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : EasyCachingAbstractProvider
    {
        /// <summary>
        /// Gets the specified cacheKey async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="type">Object Type.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public override async Task<object> BaseGetAsync(string cacheKey, Type type, CancellationToken cancellationToken = default)
        {
         ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var result = await _cache.GetAsync<byte[]>(cacheKey);
            if (result != null)
            {
                CacheStats.OnHit();

                if (_options.EnableLogging)
                    _logger?.LogInformation($"Cache Hit : cachekey = {cacheKey}");

                var value = _serializer.Deserialize(result, type);
                return value;
            }
            else
            {
                CacheStats.OnMiss();

                if (_options.EnableLogging)
                    _logger?.LogInformation($"Cache Missed : cachekey = {cacheKey}");

                return null;
            }
        }

        /// <summary>
        /// Gets the specified cacheKey, dataRetriever and expiration async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="dataRetriever">Data retriever.</param>
        /// <param name="expiration">Expiration.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public override async Task<CacheValue<T>> BaseGetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, TimeSpan expiration, CancellationToken cancellationToken = default)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNegativeOrZero(expiration, nameof(expiration));

            var result = await _cache.GetAsync<byte[]>(cacheKey);
            if (result != null)
            {
                CacheStats.OnHit();

                if (_options.EnableLogging)
                    _logger?.LogInformation($"Cache Hit : cachekey = {cacheKey}");

                var value = _serializer.Deserialize<T>(result);
                return new CacheValue<T>(value, true);
            }

            CacheStats.OnMiss();

            if (_options.EnableLogging)
                _logger?.LogInformation($"Cache Missed : cachekey = {cacheKey}");

            var flag = await _cache.SetNxAsync($"{cacheKey}_Lock", 1, (int)TimeSpan.FromMilliseconds(_options.LockMs).TotalSeconds);

            if (!flag)
            {
                await Task.Delay(_options.SleepMs);
                return await GetAsync(cacheKey, dataRetriever, expiration);
            }

            var item = await dataRetriever();
            if (item != null || _options.CacheNulls)
            {
                await SetAsync(cacheKey, item, expiration);
                //remove mutex key
                await _cache.DelAsync($"{cacheKey}_Lock");
                return new CacheValue<T>(item, true);
            }
            else
            {
                //remove mutex key
                await _cache.DelAsync($"{cacheKey}_Lock");
                return CacheValue<T>.NoValue;
            }
        }

        /// <summary>
        /// Gets the specified cacheKey async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public override async Task<CacheValue<T>> BaseGetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var result = await _cache.GetAsync<byte[]>(cacheKey);
            if (result != null)
            {
                CacheStats.OnHit();

                if (_options.EnableLogging)
                    _logger?.LogInformation($"Cache Hit : cachekey = {cacheKey}");

                var value = _serializer.Deserialize<T>(result);
                return new CacheValue<T>(value, true);
            }
            else
            {
                CacheStats.OnMiss();

                if (_options.EnableLogging)
                    _logger?.LogInformation($"Cache Missed : cachekey = {cacheKey}");

                return CacheValue<T>.NoValue;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns>The count.</returns>
        /// <param name="prefix">Prefix.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public override Task<int> BaseGetCountAsync(string prefix = "", CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the specified cacheKey async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public override async Task BaseRemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            await _cache.DelAsync(cacheKey);
        }

        /// <summary>
        /// Sets the specified cacheKey, cacheValue and expiration async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="cacheValue">Cache value.</param>
        /// <param name="expiration">Expiration.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public override async Task BaseSetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken = default)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNull(cacheValue, nameof(cacheValue), _options.CacheNulls);
            ArgumentCheck.NotNegativeOrZero(expiration, nameof(expiration));

            if (MaxRdSecond > 0)
            {
                var addSec = new Random().Next(1, MaxRdSecond);
                expiration = expiration.Add(TimeSpan.FromSeconds(addSec));
            }

            var val = _serializer.Serialize(cacheValue);

            await _cache.SetAsync(
                cacheKey,
                val,
                (int)expiration.TotalSeconds
                );
        }

        /// <summary>
        /// Existses the specified cacheKey async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public override async Task<bool> BaseExistsAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes cached item by cachekey's prefix async.
        /// </summary>
        /// <param name="prefix">Prefix of CacheKey.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public override async Task BaseRemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes cached item by pattern async.
        /// </summary>
        /// <param name="pattern">Pattern of CacheKey.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public override async Task BaseRemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
        {
            ArgumentCheck.NotNullOrWhiteSpace(pattern, nameof(pattern));

            pattern = this.HandleKeyPattern(pattern);

            if (_options.EnableLogging)
                _logger?.LogInformation($"RemoveByPatternAsync : pattern = {pattern}");

            var redisKeys = this.SearchRedisKeys(pattern);

            var tasks = new List<Task<long>>();

            foreach (var item in redisKeys)
            {
                tasks.Add(_cache.DelAsync(item));
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Sets all async.
        /// </summary>
        /// <returns>The all async.</returns>
        /// <param name="values">Values.</param>
        /// <param name="expiration">Expiration.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public override async Task BaseSetAllAsync<T>(IDictionary<string, T> values, TimeSpan expiration, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all async.
        /// </summary>
        /// <returns>The all async.</returns>
        /// <param name="cacheKeys">Cache keys.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public override async Task<IDictionary<string, CacheValue<T>>> BaseGetAllAsync<T>(IEnumerable<string> cacheKeys, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gets all keys async by prefix.
        /// </summary>
        /// <param name="prefix">Prefix</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>The all keys by prefix async.</returns>
        public override async Task<IEnumerable<string>> BaseGetAllKeysByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gets the by prefix async.
        /// </summary>
        /// <returns>The by prefix async.</returns>
        /// <param name="prefix">Prefix.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public override async Task<IDictionary<string, CacheValue<T>>> BaseGetByPrefixAsync<T>(string prefix, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all async.
        /// </summary>
        /// <returns>The all async.</returns>
        /// <param name="cacheKeys">Cache keys.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public override async Task BaseRemoveAllAsync(IEnumerable<string> cacheKeys, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Flush All Cached Item async.
        /// </summary>
        /// <returns>The async.</returns>
        public override async Task BaseFlushAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries the set async.
        /// </summary>
        /// <returns>The set async.</returns>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="cacheValue">Cache value.</param>
        /// <param name="expiration">Expiration.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public override Task<bool> BaseTrySetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the expiration of cache key
        /// </summary>
        /// <param name="cacheKey">cache key</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>expiration</returns>
        public override async Task<TimeSpan> BaseGetExpirationAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
