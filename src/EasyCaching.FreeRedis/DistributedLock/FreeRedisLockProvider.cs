namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core.DistributedLock;
    using global::FreeRedis;
    using System;
    using System.Threading.Tasks;

    public class FreeRedisLockProvider : IDistributedLockProvider
    {
        private readonly string _name;
        private readonly EasyCachingFreeRedisClient _database;

        public FreeRedisLockProvider(string name, EasyCachingFreeRedisClient database)
        {
            _name = name;
            _database = database;
        }


        public async Task<bool> SetAsync(string key, byte[] value, int ttlMs) =>
                   await _database.SetAsync(key, value, TimeSpan.FromMilliseconds(ttlMs), keepTtl: false, nx: false, xx: false, get: false) == "OK";

        public bool Add(string key, byte[] value, int ttlMs) =>
            _database.SetNx($"{_name}/{key}", value, TimeSpan.FromMilliseconds(ttlMs));

        public Task<bool> AddAsync(string key, byte[] value, int ttlMs) =>
           _database.SetNxAsync($"{_name}/{key}", value, (int)TimeSpan.FromMilliseconds(ttlMs).TotalSeconds);

        public bool Delete(string key, byte[] value) =>
            (long)_database.Eval(@"if redis.call('GET', KEYS[1]) == ARGV[1] then
    return redis.call('DEL', KEYS[1]);
end
return -1;", new[] { $"{_name}/{key}" }, value) >= 0;

        public async Task<bool> DeleteAsync(string key, byte[] value) =>
            (long)await _database.EvalAsync(@"if redis.call('GET', KEYS[1]) == ARGV[1] then
    return redis.call('DEL', KEYS[1]);
end
return -1;", new[] { $"{_name}/{key}" }, value) >= 0;

        public bool CanRetry(Exception ex) => ex is RedisClientException;
    }
}
