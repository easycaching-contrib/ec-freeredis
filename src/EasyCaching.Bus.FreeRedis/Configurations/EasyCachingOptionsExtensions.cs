namespace EasyCaching.Bus.FreeRedis
{
    using EasyCaching.Core;
    using EasyCaching.Core.Configurations;
    using Microsoft.Extensions.Configuration;
    using System;

    public static class EasyCachingOptionsExtensions
    {
        /// <summary>
        /// Withs the FreeRedis bus (specify the config via hard code).
        /// </summary>
        /// <param name="options">Options.</param>
        /// <param name="configure">Configure bus settings.</param>
        public static EasyCachingOptions WithFreeRedisBus(
            this EasyCachingOptions options
            , Action<FreeRedisBusOptions> configure
            )
        {
            ArgumentCheck.NotNull(configure, nameof(configure));

            options.RegisterExtension(new FreeRedisOptionsExtension(configure));
            return options;
        }

        /// <summary>
        /// Withs the FreeRedis bus (read config from configuration file).
        /// </summary>
        /// <param name="options">Options.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="sectionName">The section name in the configuration file.</param>
        public static EasyCachingOptions WithFreeRedisBus(
            this EasyCachingOptions options
            , IConfiguration configuration
            , string sectionName = EasyCachingConstValue.RedisBusSection
            )
        {
            var dbConfig = configuration.GetSection(sectionName);
            var redisOptions = new FreeRedisBusOptions();
            dbConfig.Bind(redisOptions);

            void configure(FreeRedisBusOptions x)
            {
                x.ConnectionStrings = redisOptions.ConnectionStrings;
            }

            options.RegisterExtension(new FreeRedisOptionsExtension(configure));
            return options;
        }
    }
}
