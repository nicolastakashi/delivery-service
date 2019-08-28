using MongoDB.Bson;

namespace DeliveryService.Domain.Queries
{
    public class BaseQuery
    {
        public ObjectId UserId { get; set; }
    }
}
