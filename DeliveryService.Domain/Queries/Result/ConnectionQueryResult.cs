using MongoDB.Bson;

namespace DeliveryService.Domain.Queries.Result
{
    public class ConnectionQueryResult
    {
        public ObjectId Id { get; set; }
        public PointQueryResult Origin { get; set; }
        public PointQueryResult Destination { get; set; }
        public decimal Time { get; set; }
        public decimal Cost { get; set; }
    }
}
