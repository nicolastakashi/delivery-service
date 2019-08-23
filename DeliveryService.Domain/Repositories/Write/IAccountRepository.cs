using DeliveryService.Domain.Entities;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Repositories.Write
{
    public interface IAccountRepository
    {
        Task<User> GetUserByEmailAndPassword(string email, string password);
    }
}
