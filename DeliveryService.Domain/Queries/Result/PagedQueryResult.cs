using System.Collections.Generic;

namespace DeliveryService.Domain.Queries.Result
{
    public class PagedQueryResult<TResult>
    {
        public PagedQueryResult(IEnumerable<TResult> values, long total, long totalOfPages)
        {
            Values = values;
            Total = total;
            TotalOfPages = totalOfPages;
        }

        public IEnumerable<TResult> Values { get; private set; }
        public long Total { get; private set; }
        public long TotalOfPages { get; private set; }

        public static PagedQueryResult<TResult> Create(IEnumerable<TResult> values, long total, long totalOfPages)
            => new PagedQueryResult<TResult>(values, total, totalOfPages);
    }
}
