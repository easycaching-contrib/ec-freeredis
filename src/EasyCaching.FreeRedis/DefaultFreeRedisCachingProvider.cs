namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using EasyCaching.Core.Serialization;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class DefaultFreeRedisCachingProvider : EasyCachingAbstractProvider
    {
        private readonly EasyCachingFreeRedisClient _cache;

        private readonly IEasyCachingSerializer _serializer;

        private readonly ILogger _logger;

        private readonly RedisOptions _options;

        private readonly CacheStats _cacheStats;

        private readonly string _name;

        private readonly ProviderInfo _info;

        public DefaultFreeRedisCachingProvider(
           string name,
           IEnumerable<EasyCachingFreeRedisClient> clients,
           IEnumerable<IEasyCachingSerializer> serializers,
           RedisOptions options,
           ILoggerFactory loggerFactory = null)
        {
            this._name = name;
            this._options = options;
            this._logger = loggerFactory?.CreateLogger<DefaultFreeRedisCachingProvider>();
            this._cache = clients.FirstOrDefault(x => x.Name.Equals(_name));

            if (this._cache == null) throw new EasyCachingNotFoundException(string.Format(EasyCachingConstValue.NotFoundCliExceptionMessage, _name));

            this._cacheStats = new CacheStats();

            var serName = !string.IsNullOrWhiteSpace(options.SerializerName) ? options.SerializerName : _name;
            this._serializer = serializers.FirstOrDefault(x => x.Name.Equals(serName));
            if (this._serializer == null) throw new EasyCachingNotFoundException(string.Format(EasyCachingConstValue.NotFoundSerExceptionMessage, serName));

            this.ProviderName = this._name;
            this.ProviderType = CachingProviderType.Redis;
            this.ProviderStats = this._cacheStats;
            this.ProviderMaxRdSecond = _options.MaxRdSecond;
            this.IsDistributedProvider = true;

            _info = new ProviderInfo
            {
                CacheStats = _cacheStats,
                EnableLogging = options.EnableLogging,
                IsDistributedProvider = IsDistributedProvider,
                LockMs = options.LockMs,
                MaxRdSecond = options.MaxRdSecond,
                ProviderName = ProviderName,
                ProviderType = ProviderType,
                SerializerName = options.SerializerName,
                SleepMs = options.SleepMs,
                Serializer = _serializer,
                CacheNulls = options.CacheNulls,
            };
        }

        public override bool BaseExists(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return _cache.Exists(cacheKey);
        }

        public override void BaseFlush()
        {
            if (_options.EnableLogging)
                _logger?.LogInformation("Redis -- Flush");

            // TODO: all nodes
            _cache.FlushDb();
        }

        public override CacheValue<T> BaseGet<T>(string cacheKey, Func<T> dataRetriever, TimeSpan expiration)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNegativeOrZero(expiration, nameof(expiration));

            var result = _cache.Get<byte[]>(cacheKey);
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

            if (!_cache.SetNx($"{cacheKey}_Lock", 1, (int)TimeSpan.FromMilliseconds(_options.LockMs).TotalSeconds))
            {
                System.Threading.Thread.Sleep(_options.SleepMs);
                return Get(cacheKey, dataRetriever, expiration);
            }

            var item = dataRetriever();
            if (item != null || _options.CacheNulls)
            {
                Set(cacheKey, item, expiration);
                //remove mutex key
                _cache.Del($"{cacheKey}_Lock");
                return new CacheValue<T>(item, true);
            }
            else
            {
                //remove mutex key
                _cache.Del($"{cacheKey}_Lock");
                return CacheValue<T>.NoValue;
            }
        }

        public override CacheValue<T> BaseGet<T>(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var result = _cache.Get<byte[]>(cacheKey);
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

        public override IDictionary<string, CacheValue<T>> BaseGetAll<T>(IEnumerable<string> cacheKeys)
        {
            ArgumentCheck.NotNullAndCountGTZero(cacheKeys, nameof(cacheKeys));

            var result = new Dictionary<string, CacheValue<T>>();

            //maybe we should use mget here based on redis mode
            //multiple keys may trigger `don't hash to the same slot`

            foreach (var item in cacheKeys)
            {
                var cachedValue = _cache.Get<byte[]>(item);
                if (cachedValue != null)
                    result.Add(item, new CacheValue<T>(_serializer.Deserialize<T>(cachedValue), true));
                else
                    result.Add(item, CacheValue<T>.NoValue);
            }

            return result;
        }

        /// <summary>
        /// Gets all keys by prefix.
        /// </summary>
        /// <param name="prefix">Prefix</param>
        /// <returns>The all keys by prefix.</returns>
        public override IEnumerable<string> BaseGetAllKeysByPrefix(string prefix)
        {
            ArgumentCheck.NotNullOrWhiteSpace(prefix, nameof(prefix));

            prefix = this.HandlePrefix(prefix);

            var redisKeys = this.GetAllRedisKeys(prefix);

            var result = redisKeys?.Select(key => (string)key)?.Distinct();

            return result;
        }

        public override IDictionary<string, CacheValue<T>> BaseGetByPrefix<T>(string prefix)
        {
            ArgumentCheck.NotNullOrWhiteSpace(prefix, nameof(prefix));

            prefix = this.HandlePrefix(prefix);

            var redisKeys = this.SearchRedisKeys(prefix);

            var result = new Dictionary<string, CacheValue<T>>();

            foreach (var item in redisKeys)
            {
                var cachedValue = _cache.Get<byte[]>(item);
                if (cachedValue != null)
                    result.Add(item, new CacheValue<T>(_serializer.Deserialize<T>(cachedValue), true));
                else
                    result.Add(item, CacheValue<T>.NoValue);
            }

            return result;
        }

        public override int BaseGetCount(string prefix = "")
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                var allCount = 0L;


                // TODO: get all nodes.
                //var servers = _cache.NodesServerManager.DbSize();

                //foreach (var item in servers)
                //{
                //    allCount += item.value;
                //}

                return (int)allCount;
            }

            return this.SearchRedisKeys(this.HandlePrefix(prefix)).Length;
        }

        public override object BaseGetDatabase() => _cache;

        public override TimeSpan BaseGetExpiration(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var second = _cache.Ttl(cacheKey);
            return TimeSpan.FromSeconds(second);
        }

        public override ProviderInfo BaseGetProviderInfo() => _info;

        public override void BaseRemove(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            _cache.Del(cacheKey);
        }

        public override void BaseRemoveAll(IEnumerable<string> cacheKeys)
        {
            ArgumentCheck.NotNullAndCountGTZero(cacheKeys, nameof(cacheKeys));

            _cache.Del(cacheKeys.ToArray());
        }

        public override void BaseRemoveByPrefix(string prefix)
        {
            ArgumentCheck.NotNullOrWhiteSpace(prefix, nameof(prefix));

            prefix = this.HandlePrefix(prefix);

            if (_options.EnableLogging)
                _logger?.LogInformation($"RemoveByPrefix : prefix = {prefix}");

            var redisKeys = this.SearchRedisKeys(prefix);

            foreach (var item in redisKeys)
            {
                _cache.Del(item);
            }
        }

        public override void BaseSet<T>(string cacheKey, T cacheValue, TimeSpan expiration)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNull(cacheValue, nameof(cacheValue), _options.CacheNulls);
            ArgumentCheck.NotNegativeOrZero(expiration, nameof(expiration));

            if (MaxRdSecond > 0)
            {
                var addSec = new Random().Next(1, MaxRdSecond);
                expiration = expiration.Add(TimeSpan.FromSeconds(addSec));
            }

            _cache.Set(
                cacheKey,
                _serializer.Serialize(cacheValue),
                (int)expiration.TotalSeconds);
        }

        public override void BaseSetAll<T>(IDictionary<string, T> values, TimeSpan expiration)
        {
            if (MaxRdSecond > 0)
            {
                var addSec = new Random().Next(1, MaxRdSecond);
                expiration = expiration.Add(TimeSpan.FromSeconds(addSec));
            }

            foreach (var item in values)
            {
                _cache.Set(item.Key, _serializer.Serialize(item.Value), (int)expiration.TotalSeconds);
            }
        }

        public override bool BaseTrySet<T>(string cacheKey, T cacheValue, TimeSpan expiration)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNull(cacheValue, nameof(cacheValue), _options.CacheNulls);
            ArgumentCheck.NotNegativeOrZero(expiration, nameof(expiration));

            if (MaxRdSecond > 0)
            {
                var addSec = new Random().Next(1, MaxRdSecond);
                expiration = expiration.Add(TimeSpan.FromSeconds(addSec));
            }

            return _cache.SetNx(
                cacheKey,
                _serializer.Serialize(cacheValue),
                (int)expiration.TotalSeconds);
        }

        /// <summary>
        /// Removes cached item by pattern async.
        /// </summary>
        /// <param name="pattern">Pattern of CacheKey.</param>
        public override void BaseRemoveByPattern(string pattern)
        {
            ArgumentCheck.NotNullOrWhiteSpace(pattern, nameof(pattern));

            pattern = this.HandleKeyPattern(pattern);

            if (_options.EnableLogging)
                _logger?.LogInformation($"RemoveByPattern : pattern = {pattern}");

            var redisKeys = this.SearchRedisKeys(pattern);

            foreach (var item in redisKeys)
            {
                _cache.Del(item);
            }
        }

        /// <summary>
        /// Handles the pattern of CacheKey.
        /// </summary>
        /// <param name="pattern">Pattern of CacheKey.</param>
        private string HandleKeyPattern(string pattern)
        {
            // Forbid
            if (pattern.Equals("*"))
                throw new ArgumentException("the pattern should not equal to *");

            var tmpPrefix = _options.DBConfig.ConnectionStrings?.FirstOrDefault()?.Prefix
              ?? _options.DBConfig.SentinelConnectionString?.Prefix;

            if (!string.IsNullOrWhiteSpace(tmpPrefix))
                pattern = tmpPrefix + pattern;

            return pattern;
        }

        private string HandlePrefix(string prefix)
        {
            // Forbid
            if (prefix.Equals("*"))
                throw new ArgumentException("the prefix should not equal to *");

            // Don't start with *
            prefix = new System.Text.RegularExpressions.Regex("^\\*+").Replace(prefix, "");

            // End with *
            if (!prefix.EndsWith("*", StringComparison.OrdinalIgnoreCase))
                prefix = string.Concat(prefix, "*");

            var tmpPrefix = _options.DBConfig.ConnectionStrings?.FirstOrDefault()?.Prefix
                ?? _options.DBConfig.SentinelConnectionString?.Prefix;

            if (!string.IsNullOrWhiteSpace(tmpPrefix))
                prefix = tmpPrefix + prefix;

            return prefix;
        }

        private string[] GetAllRedisKeys(string pattern)
        {
          throw new NotImplementedException();
        }

        /// <summary>
        /// Searchs the redis keys.
        /// </summary>
        /// <returns>The redis keys.</returns>
        /// <param name="pattern">Pattern.</param>
        private string[] SearchRedisKeys(string pattern)
        {
            var keys = new List<string>();

            long nextCursor = 0;
            do
            {
                var scanResult = _cache.Scan(nextCursor, pattern, 500, string.Empty);
                nextCursor = scanResult.cursor;
                var items = scanResult.items;
                keys.AddRange(items);
            }
            while (nextCursor != 0);

            var prefix = _options.DBConfig.ConnectionStrings?.FirstOrDefault()?.Prefix
                ?? _options.DBConfig.SentinelConnectionString?.Prefix;

            if (!string.IsNullOrWhiteSpace(prefix))
                keys = keys.Select(x => x.Remove(0, prefix.Length)).ToList();

            return keys.Distinct().ToArray();
        }
    }
}
