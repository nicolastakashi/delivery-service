using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using DeliveryService.Domain.Repositories.Readonly;
using DeliveryService.Infra.CustomExceptions;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
using DeliveryService.Infra.Data.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Repositories.ReadOnly
{
    public class RouteReadOnlyRepository : BaseReadOnlyRepository, IRouteReadOnlyRepository
    {
        private readonly IMongoContext _context;

        public RouteReadOnlyRepository(IMongoContext context, IRedisContext redisContext)
            : base(redisContext)
        {
            _context = context;
        }

        public async Task<PagedQueryResult<RoutesQueryResult>> GetAsync(GetPagedResourceQuery resource)
        {
            try
            {
                async Task<PagedQueryResult<RoutesQueryResult>> SearchInDataBaseAsync()
                {
                    var search = resource.Search.ToLower();

                    var result = await _context.GetCollection<Route>(MongoCollections.Route)
                        .AsQueryable()
                        .Where(p => p.Active)
                        .Search(p => p.Name.ToLower().Contains(search) || p.Origin.Name.ToLower().Contains(search) || p.Destination.Name.ToLower().Contains(search), resource.Search)
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => new RoutesQueryResult { Id = p.Id, Name = p.Name, Origin = p.Origin.Name, Destination = p.Destination.Name })
                        .GetPagedAsync(resource);

                    return result;
                }

                return await GetFromCacheIfExist(resource.ToCacheKey(CacheKeys.Routes), SearchInDataBaseAsync);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to get routes", ex);
            }
        }
    }
}
