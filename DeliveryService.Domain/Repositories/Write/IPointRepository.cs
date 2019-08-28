using DeliveryService.Domain.ValueObject;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Repositories.Write
{
    public interface IPointRepository : IBaseRepository<Point>, ICacheRepository
    {
        Task<Point[]> GetAllActivePoints();
    }
}
