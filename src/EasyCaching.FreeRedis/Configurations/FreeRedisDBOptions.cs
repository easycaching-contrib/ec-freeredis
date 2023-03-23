namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core.Configurations;
    using global::FreeRedis;
    using System;
    using System.Collections.Generic;

    public class FreeRedisDBOptions
    {
        public List<ConnectionStringBuilder> ConnectionStrings { get; set; }

        public List<ConnectionStringBuilder> SlaveConnectionStrings { get; set; }

        public Func<string, string> RedirectRule { get; set; } = null;

        public ConnectionStringBuilder SentinelConnectionString { get; set; }

        public List<string> Sentinels { get; set; }

        public bool RwSplitting { get; set; }

        /// <summary>
        /// Gets or sets the Redis database KeyPrefix will use.
        /// </summary>
        public string KeyPrefix { get; set; }
    }
}
