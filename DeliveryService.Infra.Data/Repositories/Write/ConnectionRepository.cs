using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;

namespace DeliveryService.Infra.Data.Repositories.Write
{
    public sealed class ConnectionRepository : BaseRepository<Connection>, IConnectionRepository
    {

        public ConnectionRepository(IMongoContext mongoContext)
            :base(mongoContext, MongoCollections.Connection)
        {
        }
    }
}
