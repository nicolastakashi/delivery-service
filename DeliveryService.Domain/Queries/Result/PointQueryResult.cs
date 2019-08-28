using MongoDB.Bson;

namespace DeliveryService.Domain.Queries.Result
{
    public class PointQueryResult
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }
    }
}
