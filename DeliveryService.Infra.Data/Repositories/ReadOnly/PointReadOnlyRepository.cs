using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using DeliveryService.Domain.Repositories.Readonly;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
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
            => await _mongoContext.GetCollection<Point>(MongoCollections.Point)
                .Find(x => x.Id == ObjectId.Parse(id) && x.Active)
                .Project(x => new PointQueryResult { Name = x.Name, Id = x.Id })
                .FirstOrDefaultAsync();

        public async Task<PagedQueryResult<PointQueryResult>> Get(GetPagedResourceQuery resource)
        {
            var query = _mongoContext.GetCollection<Point>(MongoCollections.Point)
                .Find(x => x.Active)
                .Project(x => new PointQueryResult { Id = x.Id, Name = x.Name });


            var countOfDocuments = query.CountDocumentsAsync();
            var points = query.Limit(resource.PageSize).Skip((resource.Page - 1) * resource.PageSize).ToListAsync();

            await Task.WhenAll(countOfDocuments, points);

            return PagedQueryResult<PointQueryResult>.Create(points.Result, countOfDocuments.Result, (countOfDocuments.Result / resource.PageSize));
        }
    }
}
