namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using EasyCaching.Core.Configurations;
    using EasyCaching.Core.Serialization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Linq;

    internal sealed class RedisOptionsExtension : IEasyCachingOptionsExtension
    {
       
        private readonly string _name;
       
        private readonly Action<RedisOptions> _configure;
       
        public RedisOptionsExtension(string name, Action<RedisOptions> configure)
        {
            ArgumentCheck.NotNullOrWhiteSpace(name, nameof(name));

            this._name = name;
            this._configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddOptions();

            services.TryAddSingleton<IEasyCachingSerializer, DefaultBinaryFormatterSerializer>();

            services.Configure(_name, _configure);

            services.TryAddSingleton<IEasyCachingProviderFactory, DefaultEasyCachingProviderFactory>();

            services.AddSingleton<EasyCachingFreeRedisClient>(x =>
            {
                var optionsMon = x.GetRequiredService<IOptionsMonitor<RedisOptions>>();
                var options = optionsMon.Get(_name);

                var conns = options.DBConfig.ConnectionStrings;
                var slaveConns = options.DBConfig.SlaveConnectionStrings;
                var redirectRule = options.DBConfig.RedirectRule;
                var sentinelsConn = options.DBConfig.SentinelConnectionString;
                var sentinels = options.DBConfig.Sentinels;
                var rwSplitting = options.DBConfig.RwSplitting;

                if (sentinelsConn != null)
                {
                    return new EasyCachingFreeRedisClient(_name, sentinelsConn, sentinels.ToArray(), rwSplitting);
                }

                if (conns.Count == 1)
                {
                    var slave = slaveConns != null && slaveConns.Any() ? slaveConns.ToArray() : null;
                    return new EasyCachingFreeRedisClient(_name, conns[0], slave);
                }
                else
                {
                    if(redirectRule!=null) return new EasyCachingFreeRedisClient(_name, conns.ToArray(), redirectRule);
                    else return new EasyCachingFreeRedisClient(_name, conns.ToArray());
                }
            });

            Func<IServiceProvider, DefaultFreeRedisCachingProvider> createFactory = x =>
            {
                var clients = x.GetServices<EasyCachingFreeRedisClient>();
                var serializers = x.GetServices<IEasyCachingSerializer>();
                var optionsMon = x.GetRequiredService<IOptionsMonitor<RedisOptions>>();
                var options = optionsMon.Get(_name);
                var factory = x.GetService<ILoggerFactory>();
                return new DefaultFreeRedisCachingProvider(_name, clients, serializers, options, factory);
            };

            services.AddSingleton<IEasyCachingProvider, DefaultFreeRedisCachingProvider>(createFactory);
            services.AddSingleton<IRedisCachingProvider, DefaultFreeRedisCachingProvider>(createFactory);
        }
    }
}
