using DeliveryService.Domain.Queries;

namespace DeliveryService.Infra.Data.Extensions
{
    public static class QueryResultExtensions
    {
        public static string ToCacheKey(this GetPagedResourceQuery resource, string cacheKey) 
            => string.Join(":", cacheKey, resource.Page, resource.PageSize, resource.Search);
    }
}
