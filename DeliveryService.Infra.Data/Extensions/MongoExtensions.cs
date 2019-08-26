using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Extensions
{
    public static class MongoExtensions
    {
        public static async Task<PagedQueryResult<TSource>> GetPagedAsync<TSource>(this IMongoQueryable<TSource> source, GetPagedResourceQuery resource)
        {
            var countOfDocuments = source.CountAsync();
            var points = source.Skip((resource.Page - 1) * resource.PageSize).Take(resource.PageSize).ToListAsync();

            await Task.WhenAll(countOfDocuments, points);

            return PagedQueryResult<TSource>.Create(points.Result, countOfDocuments.Result, (countOfDocuments.Result / resource.PageSize));
        }

        public static IMongoQueryable<TSource> Search<TSource>(this IMongoQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, string searchCriteria)
        {
            if (string.IsNullOrEmpty(searchCriteria)) return source;

            return source.Where(predicate);
        }
    }
}
