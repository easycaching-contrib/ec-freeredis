using FreeRedis;
using Microsoft.Extensions.DependencyInjection;

namespace EasyCaching.Bus.FreeRedis.Tests
{
    public class FreeRedisCachingBusTest
    {

        public FreeRedisCachingBusTest()
        {
             
        }


        [Fact]
        public void WithFreeRedisBus_Connectioned_Should_Succeed()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddEasyCaching(option =>
            {
                option.WithFreeRedisBus(config =>
                {
                    config.ConnectionStrings = new List<ConnectionStringBuilder>
                    {
                        "192.168.3.86:6379,defaultDatabase=6,poolsize=10"
                    };
                    config.SerializerName = "json";
                });
            });
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetService<EasyCachingFreeRedisClient>();
            Assert.NotNull(client);

            var flag = client.Ping();
            Assert.Equal("PONG", flag);
        }
    }
}