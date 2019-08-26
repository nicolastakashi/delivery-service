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
    public class RouteRepository : BaseRepository<Route>, IRouteRepository
    {
        public RouteRepository(IMongoContext mongoContext) 
            : base(mongoContext, MongoCollections.Route)
        {
        }

        public override async Task<bool> AlreadyExistsAsync(Route route)
        {
            try
            {
                return await Context.GetCollection<Route>(Collection)
                    .Find(x => x.Origin.Equals(route.Origin) && x.Destination.Equals(route.Destination) && x.Active)
                    .AnyAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to verify if point already exists", ex);
            }
        }
    }
}
