using MongoDB.Bson;

namespace DeliveryService.Domain.Entities
{
    public sealed class Edge
    {
        public ObjectId Node { get; private set; }
        public decimal Weight { get; private set; }

        public Edge()
        {
        }

        public Edge(ObjectId node, decimal weight)
        {
            Node = node;
            Weight = weight;
        }
    }
}
