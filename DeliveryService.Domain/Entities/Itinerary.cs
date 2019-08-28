using System;
using MongoDB.Bson;

namespace DeliveryService.Domain.Entities
{
    public sealed class Itinerary
    {
        public decimal Distance { get; set; }
        public ObjectId Point { get; set; }

        internal bool ShoulUpdateDistanceAndPoin(decimal calculatedDistance, bool isFirstRemove)
            => calculatedDistance < Distance || isFirstRemove;

        internal void Update(ObjectId node, decimal calculatedDistance)
        {
            Point = node;
            Distance = calculatedDistance;
        }
    }
}
