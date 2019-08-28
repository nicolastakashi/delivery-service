using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using DeliveryService.Domain.Repositories.Readonly;
using DeliveryService.Domain.ValueObject;
using DeliveryService.Infra.CustomExceptions;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
using DeliveryService.Infra.Data.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Repositories.ReadOnly
{
    public class PointReadOnlyRepository : BaseReadOnlyRepository, IPointReadOnlyRepository
    {
        private readonly IMongoContext _mongoContext;

        public PointReadOnlyRepository(IMongoContext mongoContext, IRedisContext redisContext)
            : base(redisContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<PointQueryResult> FindAsync(string id)
        {
            try
            {
                async Task<PointQueryResult> SearchInDataBaseAsync()
                {
                    return await _mongoContext.GetCollection<Point>(MongoCollections.Point)
                        .Find(x => x.Id == ObjectId.Parse(id) && x.Active)
                        .Project(x => new PointQueryResult { Name = x.Name, Id = x.Id })
                        .FirstOrDefaultAsync();
                }

                return await GetFromCacheIfExist($"{CacheKeys.Points}:{id}", SearchInDataBaseAsync);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to find the connection", ex);
            }
        }

        public async Task<PagedQueryResult<PointQueryResult>> GetAsync(GetPagedResourceQuery resource)
        {
            try
            {
                async Task<PagedQueryResult<PointQueryResult>> SearchInDataBaseAsync()
                {
                    var result = await _mongoContext.GetCollection<Point>(MongoCollections.Point)
                        .AsQueryable()
                        .Where(p => p.Active)
                        .Search(p => p.Name.ToLower().Contains(resource.Search.ToLower()), resource.Search)
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => new PointQueryResult { Id = p.Id, Name = p.Name })
                        .GetPagedAsync(resource);

                    return result;
                }

                return await GetFromCacheIfExist(resource.ToCacheKey(CacheKeys.Routes), SearchInDataBaseAsync);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to get connections", ex);
            }
        }
    }
}
