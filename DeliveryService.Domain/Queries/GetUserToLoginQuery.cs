using MongoDB.Bson;

namespace DeliveryService.Domain.Queries
{
    public class GetUserToLoginQuery
    {
        public ObjectId Id { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
    }
}
