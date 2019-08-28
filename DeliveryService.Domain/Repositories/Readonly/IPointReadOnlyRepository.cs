using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Repositories.Readonly
{
    public interface IPointReadOnlyRepository : IBaseReadOnlyRepository
    {
        Task<PagedQueryResult<PointQueryResult>> GetAsync(GetPagedResourceQuery quere);
        Task<PointQueryResult> FindAsync(string id);
    }
}
