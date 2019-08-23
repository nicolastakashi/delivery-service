using DeliveryService.Domain.Entities;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Repositories.Write
{
    public interface IPointRepository
    {
        Task CreateAsync(Point point);
        Task<Point> FindAsync(string id);
        Task<bool> PointAlreadyExistsAsync(Point point);
        Task UpdateAsync(Point point);
        Task InactivePointAsync(Point point);
    }
}
