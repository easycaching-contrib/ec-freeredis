namespace EasyCaching.Bus.FreeRedis
{
    using EasyCaching.Core;
    using EasyCaching.Core.Bus;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class DefaultFreeRedisBus : EasyCachingAbstractBus
    {
        public override void BasePublish(string topic, EasyCachingMessage message)
        {
            throw new NotImplementedException();
        }

        public override Task BasePublishAsync(string topic, EasyCachingMessage message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override void BaseSubscribe(string topic, Action<EasyCachingMessage> action)
        {
            throw new NotImplementedException();
        }

        public override Task BaseSubscribeAsync(string topic, Action<EasyCachingMessage> action, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
