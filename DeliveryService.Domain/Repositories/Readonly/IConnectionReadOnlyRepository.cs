using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Repositories.Readonly
{
    public interface IConnectionReadOnlyRepository
    {
        Task<ConnectionQueryResult> FindAsync(string id);
        Task<PagedQueryResult<ConnectionQueryResult>> GetAsync(GetPagedResourceQuery resource);
    }
}
