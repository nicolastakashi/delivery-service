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
using System;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Repositories.ReadOnly
{
    public class ConnectionReadOnlyRepository : IConnectionReadOnlyRepository
    {
        private readonly IMongoContext _mongoContext;

        public ConnectionReadOnlyRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<ConnectionQueryResult> FindAsync(string id)
        {
            try
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
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to find the connection", ex);
            }
        }

        public async Task<PagedQueryResult<ConnectionQueryResult>> GetAsync(GetPagedResourceQuery resource)
        {
            try
            {
                return await _mongoContext.GetCollection<Connection>(MongoCollections.Connection)
                    .Find(x => x.Active)
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
                    }).GetPagedAsync(resource);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to get connection paged", ex);
            }
        }
    }
}
