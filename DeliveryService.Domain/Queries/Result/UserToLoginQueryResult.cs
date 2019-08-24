using MongoDB.Bson;

namespace DeliveryService.Domain.Queries.Result
{
    public class UserToLoginQueryResult
    {
        public ObjectId Id { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
    }
}
