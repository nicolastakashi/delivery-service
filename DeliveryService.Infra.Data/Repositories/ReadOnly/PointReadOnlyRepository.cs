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
using System;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Repositories.ReadOnly
{
    public class PointReadOnlyRepository : IPointReadOnlyRepository
    {
        private readonly IMongoContext _mongoContext;

        public PointReadOnlyRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<PointQueryResult> Find(string id)
        {
            try
            {
                return await _mongoContext.GetCollection<Point>(MongoCollections.Point)
                    .Find(x => x.Id == ObjectId.Parse(id) && x.Active)
                    .Project(x => new PointQueryResult { Name = x.Name, Id = x.Id })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to find the connection", ex);
            }
        }

        public async Task<PagedQueryResult<PointQueryResult>> Get(GetPagedResourceQuery resource)
        {
            try
            {
                var result = await _mongoContext.GetCollection<Point>(MongoCollections.Point)
                    .Find(x => x.Active)
                    .Project(x => new PointQueryResult { Id = x.Id, Name = x.Name })
                    .GetPagedAsync(resource);

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to get connections", ex);
            }
        }
    }
}
