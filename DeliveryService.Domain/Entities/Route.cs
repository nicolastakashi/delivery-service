using DeliveryService.Domain.Enums;
using DeliveryService.Domain.ValueObject;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace DeliveryService.Domain.Entities
{
    public class Route : BaseEntity
    {
        public string Name { get; protected set; }
        public Point Origin { get; protected set; }
        public Point Destination { get; protected set; }
        public decimal Weight { get; protected set; }
        public UnitOfMeasure UnitOfMeasure { get; protected set; }
        public IList<Point> WayPoints { get; protected set; }

        protected Route()
        {
        }

        public Route(Point origin, Point destination)
        {
            Id = ObjectId.GenerateNewId();
            Origin = origin;
            Destination = destination;
            Name = BuildName();
            Active = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static Route Create(Point origin, Point destination)
            => new Route(origin, destination);

        private string BuildName()
            => $"From {Origin.Name} To {Destination.Name}";
    }
}
