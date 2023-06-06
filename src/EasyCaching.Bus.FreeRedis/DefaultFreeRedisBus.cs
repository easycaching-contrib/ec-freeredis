namespace EasyCaching.Bus.FreeRedis
{
    using EasyCaching.Core;
    using EasyCaching.Core.Bus;
    using EasyCaching.Core.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal class DefaultFreeRedisBus : EasyCachingAbstractBus
    {
        /// <summary>
        /// The cache.
        /// </summary>
        private readonly EasyCachingFreeRedisClient _client;

        /// <summary>
        /// The serializer.
        /// </summary>
        private readonly IEasyCachingSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EasyCaching.Bus.FreeRedis.DefaultFreeRedisBus"/> class.
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="serializer">Serializer</param>
        public DefaultFreeRedisBus(IEnumerable<EasyCachingFreeRedisClient> clients, IEasyCachingSerializer serializer)
        {
            _serializer = serializer;
            this.BusName = "easycachingbus";
            this._client = clients.FirstOrDefault(x => x.Name.Equals("easycachingbus"));
        }

        /// <summary>
        /// Publish the specified topic and message.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <param name="message">Message.</param>
        public override void BasePublish(string topic, EasyCachingMessage message)
        {
            var msg = _serializer.Serialize(message);
            _client.Publish(topic, Convert.ToBase64String(msg));
        }

        /// <summary>
        /// Publishs the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="topic">Topic.</param>
        /// <param name="message">Message.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public override async Task BasePublishAsync(string topic, EasyCachingMessage message, CancellationToken cancellationToken = default)
        {
            var msg = _serializer.Serialize(message);
            await _client.PublishAsync(topic, Convert.ToBase64String(msg));
        }

        /// <summary>
        /// Subscribe the specified topic and action.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <param name="action">Action.</param>
        public override void BaseSubscribe(string topic, Action<EasyCachingMessage> action)
        {
            _client.Subscribe(topic, OnMessage);
        }

        /// <summary>
        /// Subscribe the specified topic and action async.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <param name="action">Action.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public override async Task BaseSubscribeAsync(string topic, Action<EasyCachingMessage> action, CancellationToken cancellationToken = default)
        {
            _client.Subscribe(topic, OnMessage);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Ons the message.
        /// </summary>
        ///  <param name="topic">Topic.</param>
        /// <param name="body">Body.</param>
        private void OnMessage(string topic, object body)
        {
            var message = _serializer.Deserialize<EasyCachingMessage>(Convert.FromBase64String((string)body));
            BaseOnMessage(message);
        }
    }
}
