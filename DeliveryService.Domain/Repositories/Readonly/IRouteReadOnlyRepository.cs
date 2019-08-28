using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Repositories.Readonly
{
    public interface IRouteReadOnlyRepository : IBaseReadOnlyRepository
    {
        Task<PagedQueryResult<RoutesQueryResult>> GetAsync(GetPagedResourceQuery resource);
    }
}
