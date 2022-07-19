namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core.Configurations;

    public class RedisOptions : BaseProviderOptions
    {
        public RedisOptions()
        {

        }

        /// <summary>
        /// Gets or sets the DB Config.
        /// </summary>
        /// <value>The DBC onfig.</value>
        public FreeRedisDBOptions DBConfig { get; set; } = new FreeRedisDBOptions();
    }
}
