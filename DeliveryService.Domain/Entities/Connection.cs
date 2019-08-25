using MongoDB.Bson;
using System;

namespace DeliveryService.Domain.Entities
{
    public class Connection : BaseEntity
    {
        public Point Origin { get; protected set; }
        public Point Destination { get; protected set; }
        public decimal Time { get; protected set; }
        public decimal Cost { get; protected set; }

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
    }
}
