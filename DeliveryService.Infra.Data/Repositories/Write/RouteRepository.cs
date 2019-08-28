using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Infra.CustomExceptions;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
using System;

namespace DeliveryService.Infra.Data.Repositories.Write
{
    public class RouteRepository : BaseRepository<Route>, IRouteRepository
    {
        private readonly IRedisContext _redisContext;

        public RouteRepository(IMongoContext mongoContext, IRedisContext redisContext)
            : base(mongoContext, MongoCollections.Route)
        {
            _redisContext = redisContext;
        }

        public void ClearCache()
        {
            try
            {
                _redisContext.DeleteAllByPattern($"{CacheKeys.Routes}:*");
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Erro to clear route cache", ex);
            }
        }
    }
}
