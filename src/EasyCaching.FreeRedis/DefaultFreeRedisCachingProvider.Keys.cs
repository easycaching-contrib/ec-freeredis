namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public string RedisName => this._name;

        public bool KeyDel(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return _cache.Del(cacheKey) > 0;
        }

        public async Task<bool> KeyDelAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return await _cache.DelAsync(cacheKey) > 0;
        }

        public bool KeyExpire(string cacheKey, int second)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return _cache.Expire(cacheKey, second);
        }

        public async Task<bool> KeyExpireAsync(string cacheKey, int second)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return await _cache.ExpireAsync(cacheKey, second);
        }

        public bool KeyPersist(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return _cache.Persist(cacheKey);
        }

        public async Task<bool> KeyPersistAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return await _cache.PersistAsync(cacheKey);
        }

        public bool KeyExists(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return _cache.Exists(cacheKey);
        }

        public async Task<bool> KeyExistsAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return await _cache.ExistsAsync(cacheKey);
        }

        public long TTL(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return _cache.Ttl(cacheKey);
        }

        public async Task<long> TTLAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return await _cache.TtlAsync(cacheKey);
        }

        public object Eval(string script, string cacheKey, List<object> args)
        {
            ArgumentCheck.NotNullOrWhiteSpace(script, nameof(script));
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = _cache.Eval(script, new[] { cacheKey }, args.ToArray());
            return res;
        }

        public async Task<object> EvalAsync(string script, string cacheKey, List<object> args)
        {
            ArgumentCheck.NotNullOrWhiteSpace(script, nameof(script));
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = await _cache.EvalAsync(script, new[] { cacheKey }, args.ToArray());
            return res;
        }

        public List<string> SearchKeys(string cacheKey, int? count)
        {
            var keys = new List<string>();

            long nextCursor = 0;
            do
            {
                var scanResult = _cache.Scan(nextCursor, cacheKey, count ?? 250, string.Empty);
                nextCursor = scanResult.cursor;
                var items = scanResult.items;
                keys.AddRange(items);
            }
            while (nextCursor != 0);

            return keys;
        }

        public async Task<List<string>> SearchKeysAsync(string cacheKey, int? count)
        {
            var keys = new List<string>();

            long nextCursor = 0;
            do
            {
                var scanResult = await _cache.ScanAsync(nextCursor, cacheKey, count ?? 250, string.Empty);
                nextCursor = scanResult.cursor;
                var items = scanResult.items;
                keys.AddRange(items);
            }
            while (nextCursor != 0);

            return keys;
        }
    }
}
