namespace DeliveryService.Domain.Repositories.Readonly
{
    public interface IBaseReadOnlyRepository
    {
        void SaveOnCache<TSource>(string cacheKey, TSource source);
    }
}