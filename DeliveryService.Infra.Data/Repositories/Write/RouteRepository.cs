using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;

namespace DeliveryService.Infra.Data.Repositories.Write
{
    public class RouteRepository : BaseRepository<Route>, IRouteRepository
    {
        public RouteRepository(IMongoContext mongoContext) 
            : base(mongoContext, MongoCollections.Route)
        {
        }
    }
}
