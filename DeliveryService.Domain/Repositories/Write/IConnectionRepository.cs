using DeliveryService.Domain.Entities;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Repositories.Write
{
    public interface IConnectionRepository : IBaseRepository<Connection>, ICacheRepository
    {
        Task<Connection[]> GetAllActiveConnections();
    }
}
