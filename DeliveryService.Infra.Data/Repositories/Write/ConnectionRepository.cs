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

        public ConnectionRepository(IMongoContext mongoContext)
            :base(mongoContext, MongoCollections.Connection)
        {
        }

        public override async Task<bool> AlreadyExistsAsync(Connection connection)
        {
            try
            {
                return await Context.GetCollection<Connection>(Collection)
                .Find(x => x.Origin.Equals(connection.Origin) && x.Destination.Equals(connection.Destination) && x.Active)
                .AnyAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to verify if connection already exists", ex);
            }
        }
    }
}
