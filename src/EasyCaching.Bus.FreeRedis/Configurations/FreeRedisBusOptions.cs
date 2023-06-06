namespace EasyCaching.Bus.FreeRedis
{
    using System;
    using System.Collections.Generic;
    using global::FreeRedis;

    public class FreeRedisBusOptions
    {
        public List<ConnectionStringBuilder> ConnectionStrings { get; set; }

        public List<ConnectionStringBuilder> SlaveConnectionStrings { get; set; }

        /// <summary>
        /// Redirect rule
        /// </summary>
        public Func<string, string> RedirectRule { get; set; } = null;

        /// <summary>
        /// Sentinel master connection string.
        /// </summary>
        public ConnectionStringBuilder SentinelConnectionString { get; set; }

        /// <summary>
        /// Redis sentinels.
        /// </summary>
        public List<string> Sentinels { get; set; }

        /// <summary>
        /// This variable indicates whether to use the read-write separation mode.
        /// </summary>
        public bool RwSplitting { get; set; }

        /// <summary>
        /// Gets or sets the Redis database KeyPrefix will use.
        /// </summary>
        public string KeyPrefix { get; set; }
    }
}
