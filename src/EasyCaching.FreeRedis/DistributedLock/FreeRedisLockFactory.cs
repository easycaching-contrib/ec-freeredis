namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core.DistributedLock;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Linq;

    public class FreeRedisLockFactory : DistributedLockFactory
    {
        private readonly IEnumerable<EasyCachingFreeRedisClient> _clients;

        public FreeRedisLockFactory(IEnumerable<EasyCachingFreeRedisClient> clients,
            IOptionsMonitor<RedisOptions> optionsMonitor,
            ILoggerFactory loggerFactory = null)
            : base(name => DistributedLockOptions.FromProviderOptions(optionsMonitor.Get(name)), loggerFactory) =>
            _clients = clients;


        protected override IDistributedLockProvider GetLockProvider(string name) =>
            new FreeRedisLockProvider(name, _clients.Single(x => x.Name.Equals(name)));
    }
}
