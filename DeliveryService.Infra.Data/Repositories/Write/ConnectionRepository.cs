using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Infra.CustomExceptions;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Repositories.Write
{
    public sealed class ConnectionRepository : IConnectionRepository
    {
        private readonly IMongoContext _mongoContext;

        public ConnectionRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<bool> AlreadyExists(Connection connection)
        {
            try
            {
                return await _mongoContext.GetCollection<Connection>(MongoCollections.Connection)
                .Find(x => x.Origin.Id == connection.Origin.Id && x.Destination.Id == connection.Destination.Id)
                .AnyAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to verify if connection already exists", ex);
            }
        }

        public async Task CreateAsync(Connection connection)
        {
            try
            {
                await _mongoContext.GetCollection<Connection>(MongoCollections.Connection).InsertOneAsync(connection);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to create the connection", ex);
            }
        }

        public async Task<Connection> FindAsync(string id)
        {
            try
            {
                return await _mongoContext.GetCollection<Connection>(MongoCollections.Connection)
                    .Find(x => x.Id == ObjectId.Parse(id))
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to find the connection", ex);
            }
        }


        public async Task UpdateAsync(Connection connection)
        {
            try
            {
                await _mongoContext.GetCollection<Connection>(MongoCollections.Connection).FindOneAndReplaceAsync(x => x.Id == connection.Id, connection);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to update the connection", ex);
            }
        }
    }
}
