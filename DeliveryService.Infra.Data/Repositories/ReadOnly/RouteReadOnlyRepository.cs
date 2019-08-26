using System;
using System.Threading.Tasks;
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

namespace DeliveryService.Infra.Data.Repositories.ReadOnly
{
    public class RouteReadOnlyRepository : IRouteReadOnlyRepository
    {
        private readonly IMongoContext _context;

        public RouteReadOnlyRepository(IMongoContext context)
        {
            _context = context;
        }

        public async Task<PagedQueryResult<RoutesQueryResult>> GetAsync(GetPagedResourceQuery resource)
        {
            try
            {
                var search = resource.Search.ToLower();

                var result = await _context.GetCollection<Route>(MongoCollections.Point)
                    .AsQueryable()
                    .Where(p => p.Active)
                    .Search(p => p.Name.ToLower().Contains(search) || p.Origin.Name.ToLower().Contains(search) || p.Destination.Name.ToLower().Contains(search), resource.Search)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new RoutesQueryResult { Id = p.Id, Name = p.Name, Origin = p.Origin.Name, Destination = p.Destination.Name })
                    .GetPagedAsync(resource);

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to get routes", ex);
            }
        }
    }
}
