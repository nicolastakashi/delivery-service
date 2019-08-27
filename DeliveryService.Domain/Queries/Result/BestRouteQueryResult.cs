using DeliveryService.Domain.Enums;
using MongoDB.Bson;
using System.Collections.Generic;

namespace DeliveryService.Domain.Queries.Result
{
    public class BestRouteQueryResult
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public PointQueryResult Origin { get; set; }
        public PointQueryResult Destination { get; set; }
        public IEnumerable<PointQueryResult> WayPoints { get; set; }
        public decimal Weight { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
    }
}
