using DeliveryService.Domain.Queries;
using DeliveryService.Domain.Queries.Result;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Repositories.Readonly
{
    public interface IPointReadOnlyRepository
    {
        Task<PagedQueryResult<PointQueryResult>> Get(GetPagedResourceQuery quere);
        Task<PointQueryResult> Find(string id);
    }
}
