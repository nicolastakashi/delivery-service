using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;
using System.Linq;

namespace DeliveryService.Infra.Data.Context
{
    public class RedisContext : IRedisContext
    {
        protected readonly IRedisClient Client;

        public RedisContext(IConfiguration configuration)
        {
            Client = new RedisClient(configuration.GetConnectionString("Redis"));
        }

        public void DeleteAllByPattern(string pattern)
        {
            var keys = Client.GetKeysByPattern(pattern);
            if (keys.Any()) Client.RemoveAll(keys);
        }

        public TEntity Get<TEntity>(string key)
            => Client.Get<TEntity>(key);

        public bool Save<TEntity>(string key, TEntity value)
            => Client.Set(key, value);
    }
}
