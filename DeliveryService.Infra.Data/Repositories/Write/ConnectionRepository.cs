using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Infra.CustomExceptions;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Repositories.Write
{
    public sealed class ConnectionRepository : BaseRepository<Connection>, IConnectionRepository
    {
        private readonly IRedisContext _redisContext;

        public ConnectionRepository(IMongoContext mongoContext, IRedisContext redisContext)
            : base(mongoContext, MongoCollections.Connection)
        {
            _redisContext = redisContext;
        }

        public void ClearCache()
        {
            try
            {
                _redisContext.DeleteAllByPattern($"{CacheKeys.Connections}:*");
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Erro to clear connection cache", ex);
            }
        }

        public async Task<Connection[]> GetAllActiveConnections()
        {
            try
            {
                var connections = await Collection.Find(x => x.Active).ToListAsync();

                return connections.ToArray();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to get all active connections", ex);
            }
        }
    }
}
