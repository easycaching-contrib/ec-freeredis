namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using EasyCaching.Core.Configurations;
    using EasyCaching.Core.DistributedLock;
    using Microsoft.Extensions.Configuration;
    using System;

    public static class EasyCachingOptionsExtensions
    {
        public const string DefaultFreeRedisName = "DefaultFreeRedis";
        public const string DefaultFreeRedisSection = "easycaching:freeredis";

        public static EasyCachingOptions UseFreeRedis(
            this EasyCachingOptions options
            , Action<RedisOptions> configure
            , string name = DefaultFreeRedisName)
        {
            ArgumentCheck.NotNull(configure, nameof(configure));

            options.RegisterExtension(new RedisOptionsExtension(name, configure));
            return options;
        }

        public static EasyCachingOptions UseFreeRedis(
            this EasyCachingOptions options
            , IConfiguration configuration
            , string name = DefaultFreeRedisName
            , string sectionName = DefaultFreeRedisSection)
        {
            var dbConfig = configuration.GetSection(sectionName);
            var redisOptions = new RedisOptions();
            dbConfig.Bind(redisOptions);

            void configure(RedisOptions x)
            {
                x.EnableLogging = redisOptions.EnableLogging;
                x.MaxRdSecond = redisOptions.MaxRdSecond;
                x.LockMs = redisOptions.LockMs;
                x.SleepMs = redisOptions.SleepMs;
                x.SerializerName = redisOptions.SerializerName;
                x.CacheNulls = redisOptions.CacheNulls;
                x.DBConfig = redisOptions.DBConfig;
            }

            options.RegisterExtension(new RedisOptionsExtension(name, configure));
            return options;
        }

        /// <summary>
        /// Uses the FreeRedis lock.
        /// </summary>
        /// <param name="options">Options.</param>
        public static EasyCachingOptions UseRedisLock(this EasyCachingOptions options)
        {
            options.UseDistributedLock<FreeRedisLockFactory>();

            return options;
        }
    }
}
