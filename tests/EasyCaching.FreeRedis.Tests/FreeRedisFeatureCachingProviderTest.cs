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
            services.AddEasyCaching(ecops =>
               ecops.UseFreeRedis(frops =>
               {
                   frops.DBConfig = new FreeRedisDBOptions
                   {
                       ConnectionStrings = new List<ConnectionStringBuilder>
                       {
                            "192.168.3.86:6379,defaultDatabase=13,poolsize=10"
                       }
                   };
               }, ProviderName).UseRedisLock().WithJson(ProviderName));

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            _provider = serviceProvider.GetService<IRedisCachingProvider>();
            _baseProvider = serviceProvider.GetService<IEasyCachingProvider>();
            _nameSpace = "FreeRedisFeature";
        }
    }
}
