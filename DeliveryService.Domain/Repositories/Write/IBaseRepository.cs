using DeliveryService.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Repositories.Write
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<bool> AlreadyExistsAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> FindAsync(string id);
    }
}
