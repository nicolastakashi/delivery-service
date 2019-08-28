using System;
using MongoDB.Bson;

namespace DeliveryService.Domain.Entities
{
    public sealed class Itinerary
    {
        public decimal Distance { get; set; }
        public ObjectId Point { get; set; }

        public Itinerary(ObjectId point, decimal distance)
        {
            Point = point;
            Distance = distance;
        }

        internal bool ShouldUpdateDistanceAndPoint(decimal calculatedDistance, bool isFirstRemove)
            => calculatedDistance < Distance || isFirstRemove;

        internal void Update(ObjectId node, decimal calculatedDistance)
        {
            Point = node;
            Distance = calculatedDistance;
        }
    }
}
