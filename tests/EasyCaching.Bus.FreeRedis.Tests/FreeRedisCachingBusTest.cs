using Castle.DynamicProxy.Generators;
using EasyCaching.Core.Bus;
using FreeRedis;
using Microsoft.Extensions.DependencyInjection;

namespace EasyCaching.Bus.FreeRedis.Tests;

public class FreeRedisCachingBusTest
{

    private const string Topic = "test-topic";
    private IEasyCachingBus _bus;

    public FreeRedisCachingBusTest()
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

            option.WithJson("json");
        });

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        _bus = serviceProvider.GetService<IEasyCachingBus>()!;
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
            });
        });
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetService<EasyCachingFreeRedisClient>();
        Assert.NotNull(client);

        var flag = client.Ping();
        Assert.Equal("PONG", flag);
    }

    [Fact]
    public async Task Publish_Msg_Should_Succeed()
    {
        var message = new EasyCachingMessage
        {
            Id = Guid.NewGuid().ToString("N"),
            CacheKeys = new string[] { "freeredis:bus:cachekey" }
        };
        await _bus.PublishAsync(Topic, message);

        Assert.True(true);
    }

    [Fact]
    public async Task Publish_Msg_And_Subscribe_Should_Succeed()
    {
        var sendMsgCachkey = "freeredis:bus:cachekey";
        var message = new EasyCachingMessage
        {
            Id = Guid.NewGuid().ToString("N"),
            CacheKeys = new string[] { sendMsgCachkey }
        };
        var getMsgCachkey = string.Empty;
        await _bus.SubscribeAsync(Topic,
            msg =>
            {
                getMsgCachkey = msg.CacheKeys[0];
            },
            () => { });

        await _bus.PublishAsync(Topic, message);
        await Task.Delay(1000);
        Assert.Equal(sendMsgCachkey, getMsgCachkey);
    }
}