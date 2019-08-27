using MongoDB.Bson;

namespace DeliveryService.Domain.Entities
{
    public sealed class Itinerary
    {
        public decimal Distance { get; set; }
        public ObjectId Point { get; set; }
    }
}
