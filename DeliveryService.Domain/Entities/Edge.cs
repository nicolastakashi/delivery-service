using MongoDB.Bson;

namespace DeliveryService.Domain.Entities
{
    public sealed class Edge
    {
        public ObjectId Id { get; private set; }
        public decimal Weight { get; private set; }

        public Edge()
        {
        }

        public Edge(ObjectId node, decimal weight)
        {
            Id = node;
            Weight = weight;
        }
    }
}
