using DeliveryService.Domain.Entities;
using DeliveryService.Domain.ValueObject;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Repositories.Write
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<bool> AlreadyExistsAsync(TEntity entity);
        Task<TEntity> FindAsync(string id);
    }
}
