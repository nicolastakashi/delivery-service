using System;
using System.Threading.Tasks;
using DeliveryService.Domain.Repositories.Readonly;
using DeliveryService.Infra.Data.Context;

namespace DeliveryService.Infra.Data.Repositories.ReadOnly
{
    public class BaseReadOnlyRepository : IBaseReadOnlyRepository
    {
        private IRedisContext _redisContext;

        public BaseReadOnlyRepository(IRedisContext redisContext)
        {
            _redisContext = redisContext;
        }

        internal async Task<TSource> GetFromCacheIfExist<TSource>(string cacheKey, Func<Task<TSource>> callBack)
        {
            var items = _redisContext.Get<TSource>(cacheKey);

            if (items == null)
            {
                items = await callBack();

                SaveOnCache(cacheKey, items);
            }

            return items;
        }

        public void SaveOnCache<TSource>(string cacheKey, TSource source)
        {
            _redisContext.Save(cacheKey, source);
        }
    }
}
