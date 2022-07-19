namespace EasyCaching.FreeRedis
{
    using global::FreeRedis;
    using System;

    public class EasyCachingFreeRedisClient : RedisClient
    {
        private readonly string _name;

        public string Name { get { return this._name; } }

        public EasyCachingFreeRedisClient(string name, ConnectionStringBuilder connectionString, params ConnectionStringBuilder[] slaveConnectionStrings)
            : base(connectionString, slaveConnectionStrings)
        {
            this._name = name;
        }

        public EasyCachingFreeRedisClient(string name, ConnectionStringBuilder[] clusterConnectionStrings)
            : base(clusterConnectionStrings)
        {
            this._name = name;
        }

        public EasyCachingFreeRedisClient(string name, ConnectionStringBuilder[] connectionStrings, Func<string, string> redirectRule)
           : base(connectionStrings, redirectRule)
        {
            this._name = name;
        }

        public EasyCachingFreeRedisClient(string name, ConnectionStringBuilder sentinelConnectionString, string[] sentinels, bool rw_splitting)
          : base(sentinelConnectionString, sentinels, rw_splitting)
        {
            this._name = name;
        }
    }
}
