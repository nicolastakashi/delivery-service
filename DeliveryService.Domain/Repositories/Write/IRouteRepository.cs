using DeliveryService.Domain.Entities;

namespace DeliveryService.Domain.Repositories.Write
{
    public interface IRouteRepository : IBaseRepository<Route>, ICacheRepository
    {
    }
}
