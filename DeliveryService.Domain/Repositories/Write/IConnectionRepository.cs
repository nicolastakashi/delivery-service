using DeliveryService.Domain.Entities;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Repositories.Write
{
    public interface IConnectionRepository
    {
        Task CreateAsync(Connection connection);
        Task UpdateAsync(Connection connection);
        Task<Connection> FindAsync(string id);
        Task<bool> AlreadyExists(Connection connection);
    }
}
