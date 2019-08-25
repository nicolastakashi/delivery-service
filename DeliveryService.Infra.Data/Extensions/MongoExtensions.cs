using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Extensions
{
    public static class MongoExtensions
    {
        public static async Task<PagedQueryResult<TNewProjection>> GetPagedAsync<TDocument, TNewProjection>(this IFindFluent<TDocument, TNewProjection> findFluent, GetPagedResourceQuery resource)
        {
            var countOfDocuments = findFluent.CountDocumentsAsync();
            var points = findFluent.Limit(resource.PageSize).Skip((resource.Page - 1) * resource.PageSize).ToListAsync();

            await Task.WhenAll(countOfDocuments, points);

            return PagedQueryResult<TNewProjection>.Create(points.Result, countOfDocuments.Result, (countOfDocuments.Result / resource.PageSize));
        }
    }
}
