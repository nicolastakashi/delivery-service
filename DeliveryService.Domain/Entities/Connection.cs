using DeliveryService.Domain.ValueObject;
using MongoDB.Bson;
using System;

namespace DeliveryService.Domain.Entities
{
    public sealed class Connection : BaseEntity
    {
        public Point Origin { get; private set; }
        public Point Destination { get; private set; }
        public decimal Time { get; private set; }
        public decimal Cost { get; private set; }

        public Connection(Point origin, Point destination,decimal time, decimal cost )
        {
            Id = ObjectId.GenerateNewId();
            Origin = origin;
            Destination = destination;
            Time = time;
            Cost = cost;
            Active = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Update(Point origin, Point destination, decimal time, decimal cost)
        {
            Origin = origin;
            Destination = destination;
            Time = time;
            Cost = cost;
            UpdatedAt = DateTime.UtcNow;
        }

        internal bool ArePointsChanged(Point origin, Point destination) 
            => Origin.Equals(origin) is false || Destination.Equals(destination) is false;
    }
}
