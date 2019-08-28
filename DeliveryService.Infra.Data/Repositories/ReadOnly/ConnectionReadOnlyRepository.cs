using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using DeliveryService.Domain.Repositories.Readonly;
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
    public class ConnectionReadOnlyRepository : BaseReadOnlyRepository, IConnectionReadOnlyRepository
    {
        private readonly IMongoContext _mongoContext;

        public ConnectionReadOnlyRepository(IMongoContext mongoContext, IRedisContext redisContext)
            : base(redisContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<ConnectionQueryResult> FindAsync(string id)
        {
            try
            {
                async Task<ConnectionQueryResult> SearchInDataBaseAsync()
                {
                    return await _mongoContext.GetCollection<Connection>(MongoCollections.Connection)
                        .Find(x => x.Id == ObjectId.Parse(id) && x.Active)
                        .Project(x => new ConnectionQueryResult
                        {
                            Id = x.Id,
                            Cost = x.Cost,
                            Time = x.Time,
                            Origin = new PointQueryResult
                            {
                                Id = x.Origin.Id,
                                Name = x.Origin.Name
                            },
                            Destination = new PointQueryResult
                            {
                                Id = x.Destination.Id,
                                Name = x.Destination.Name
                            }
                        }).FirstOrDefaultAsync();
                }
                return await GetFromCacheIfExist($"{CacheKeys.Routes}:{id}", SearchInDataBaseAsync);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to find the connection", ex);
            }
        }

        public async Task<PagedQueryResult<ConnectionQueryResult>> GetAsync(GetPagedResourceQuery resource)
        {
            try
            {
                async Task<PagedQueryResult<ConnectionQueryResult>> SearchInDataBaseAsync()
                {
                    var search = resource.Search.ToLower();

                    return await _mongoContext.GetCollection<Connection>(MongoCollections.Connection)
                        .AsQueryable()
                        .Where(x => x.Active)
                        .Search(x => x.Origin.Name.ToLower().Contains(search) || x.Destination.Name.ToLower().Contains(search), search)
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(x => new ConnectionQueryResult
                        {
                            Id = x.Id,
                            Cost = x.Cost,
                            Time = x.Time,
                            Origin = new PointQueryResult
                            {
                                Id = x.Origin.Id,
                                Name = x.Origin.Name
                            },
                            Destination = new PointQueryResult
                            {
                                Id = x.Destination.Id,
                                Name = x.Destination.Name
                            }
                        }).GetPagedAsync(resource);
                }

                return await GetFromCacheIfExist(resource.ToCacheKey(CacheKeys.Connections), SearchInDataBaseAsync);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to get connection paged", ex);
            }
        }
    }
}
