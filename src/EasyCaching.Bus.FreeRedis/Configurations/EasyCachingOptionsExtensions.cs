namespace EasyCaching.Bus.FreeRedis
{
    using EasyCaching.Core;
    using EasyCaching.Core.Configurations;
    using Microsoft.Extensions.Configuration;
    using System;

    public static class EasyCachingOptionsExtensions
    {
        private const string DefaultBusName = "easycachingbus";

        /// <summary>
        /// Withs the FreeRedis bus (specify the config via hard code).
        /// </summary>
        /// <param name="options">Options.</param>
        /// <param name="configure">Configure bus settings.</param>
        /// <param name="name">Bus Name.</param>
        public static EasyCachingOptions WithFreeRedisBus(
            this EasyCachingOptions options
            , Action<FreeRedisBusOptions> configure
            , string name = DefaultBusName
            )
        {
            ArgumentCheck.NotNull(configure, nameof(configure));

            options.RegisterExtension(new FreeRedisOptionsExtension(name, configure));
            return options;
        }

        /// <summary>
        /// Withs the FreeRedis bus (read config from configuration file).
        /// </summary>
        /// <param name="options">Options.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="name">Bus Name.</param>
        /// <param name="sectionName">The section name in the configuration file.</param>
        public static EasyCachingOptions WithFreeRedisBus(
            this EasyCachingOptions options
            , IConfiguration configuration
            , string name = DefaultBusName
            , string sectionName = EasyCachingConstValue.RedisBusSection
            )
        {
            var dbConfig = configuration.GetSection(sectionName);
            var redisOptions = new FreeRedisBusOptions();
            dbConfig.Bind(redisOptions);

            void configure(FreeRedisBusOptions x)
            {
                x.ConnectionStrings = redisOptions.ConnectionStrings;
                x.SlaveConnectionStrings = redisOptions.SlaveConnectionStrings;
                x.RedirectRule = redisOptions.RedirectRule;
                x.SentinelConnectionString = redisOptions.SentinelConnectionString;
                x.Sentinels = redisOptions.Sentinels;
                x.RwSplitting = redisOptions.RwSplitting;
                x.KeyPrefix = redisOptions.KeyPrefix;
                x.SerializerName = redisOptions.SerializerName;
            }

            options.RegisterExtension(new FreeRedisOptionsExtension(name, configure));
            return options;
        }
    }
}
