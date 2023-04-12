namespace EasyCaching.FreeRedis.Tests
{
    using EasyCaching.Core;
    using global::FreeRedis;
    using Microsoft.Extensions.DependencyInjection;

    public class FreeRedisFeatureCachingProviderTest : BaseRedisFeatureCachingProviderTest
    {
        private readonly string ProviderName = "Test";

        public FreeRedisFeatureCachingProviderTest()
        {
            IServiceCollection services = new ServiceCollection();
            // **************** Pooling Test ****************
            services.AddEasyCaching(ecops =>
                ecops.UseFreeRedis(frops =>
                {
                    frops.DBConfig = new FreeRedisDBOptions
                    {
                        ConnectionStrings = new List<ConnectionStringBuilder>
                        {
                            "127.0.0.1,defaultDatabase=13,poolsize=10",
                        }
                    };
                }, ProviderName).UseRedisLock().WithJson(ProviderName));

            // **************** Cluster Test ****************
            //services.AddEasyCaching(ecops =>
            //    ecops.UseFreeRedis(frops =>
            //    {
            //        frops.DBConfig = new FreeRedisDBOptions
            //        {
            //            ConnectionStrings = new List<ConnectionStringBuilder>
            //            {
            //                "127.0.0.1:7000","127.0.0.1:7001","127.0.0.1:7002",
            //            }
            //        };
            //    }, ProviderName).UseRedisLock().WithJson(ProviderName));

            // **************** Sentinel Test ****************
            //services.AddEasyCaching(ecops =>
            //    ecops.UseFreeRedis(frops =>
            //    {
            //        frops.DBConfig = new FreeRedisDBOptions
            //        {
            //            SentinelConnectionString = "mymaster",
            //            Sentinels = new List<string> { "127.0.0.1:26379", "127.0.0.1:26380" },
            //            RwSplitting = true
            //        };
            //    }, ProviderName).UseRedisLock().WithJson(ProviderName));

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            _provider = serviceProvider.GetService<IRedisCachingProvider>();
            _baseProvider = serviceProvider.GetService<IEasyCachingProvider>();
            _nameSpace = "FreeRedisFeature";
        }
    }
}
