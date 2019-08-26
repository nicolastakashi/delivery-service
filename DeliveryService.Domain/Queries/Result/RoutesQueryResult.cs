using MongoDB.Bson;

namespace DeliveryService.Domain.Queries.Result
{
    public class RoutesQueryResult
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
    }
}
