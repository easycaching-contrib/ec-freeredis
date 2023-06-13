namespace EasyCaching.Bus.FreeRedis
{
    using EasyCaching.Core;
    using EasyCaching.Core.Bus;
    using EasyCaching.Core.Configurations;
    using EasyCaching.Core.Serialization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;
    using System;
    using System.Linq;

    public class FreeRedisOptionsExtension : IEasyCachingOptionsExtension
    {
        /// <summary>
        /// The name.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// The configure.
        /// </summary>
        private readonly Action<FreeRedisBusOptions> _configure;

        /// <summary>
        /// <param name="name">Name.</param>
        /// Initializes a new instance of the <see cref="T:EasyCaching.Bus.FreeRedis.Configurations.FreeRedisBusOptions"/> class.
        /// </summary>        
        /// <param name="configure">Configure.</param>
        public FreeRedisOptionsExtension(string name, Action<FreeRedisBusOptions> configure)
        {
            this._name = name;
            this._configure = configure;
        }


        public void AddServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure(_name, _configure);
            services.TryAddSingleton<IEasyCachingProviderFactory, DefaultEasyCachingProviderFactory>();

            services.AddSingleton<EasyCachingFreeRedisClient>(x =>
            {
                var optionsMon = x.GetRequiredService<IOptionsMonitor<FreeRedisBusOptions>>();
                var options = optionsMon.Get(_name);

                var conns = options.ConnectionStrings;
                var slaveConns = options.SlaveConnectionStrings;
                var redirectRule = options.RedirectRule;
                var sentinelsConn = options.SentinelConnectionString;
                var sentinels = options.Sentinels;
                var rwSplitting = options.RwSplitting;

                if (sentinelsConn != null)
                {
                    // Redis Sentinel
                    return new EasyCachingFreeRedisClient(_name, sentinelsConn, sentinels.ToArray(), rwSplitting);
                }

                if (conns.Count == 1)
                {
                    // Pooling RedisClient
                    var slave = slaveConns != null && slaveConns.Any() ? slaveConns.ToArray() : null;
                    return new EasyCachingFreeRedisClient(_name, conns[0], slave);
                }
                else
                {
                    //  Norman RedisClient Or Redis Cluster
                    if (redirectRule != null) return new EasyCachingFreeRedisClient(_name, conns.ToArray(), redirectRule);
                    else return new EasyCachingFreeRedisClient(_name, conns.ToArray());
                }
            });

            services.AddSingleton<IEasyCachingBus, DefaultFreeRedisBus>(x =>
            {
                var clients = x.GetServices<EasyCachingFreeRedisClient>();
                var optionsMon = x.GetRequiredService<IOptionsMonitor<FreeRedisBusOptions>>();
                var options = optionsMon.Get(_name);
                var serializers = x.GetServices<IEasyCachingSerializer>();
                return new DefaultFreeRedisBus(_name, clients, options, serializers);
            });
        }
    }
}
