using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Domain.ValueObject;
using DeliveryService.Infra.CustomExceptions;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Repositories.Write
{
    public class PointRepository : BaseRepository<Point>, IPointRepository
    {
        private readonly IRedisContext _redisContext;

        public PointRepository(IMongoContext context, IRedisContext redisContext)
            : base(context, MongoCollections.Point)
        {
            _redisContext = redisContext;
        }

        public void ClearCache()
        {
            try
            {
                _redisContext.DeleteAllByPattern($"{CacheKeys.Points}:*");
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Erro to clear point cache", ex);
            }
        }

        public async Task<Point[]> GetAllActivePoints()
        {
            try
            {
                var points = await Collection.Find(x => x.Active).ToListAsync();
                return points.ToArray();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to get all active points", ex);
            }
        }
    }
}
