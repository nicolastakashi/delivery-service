using DeliveryService.Domain.ValueObject;
using MongoDB.Bson;
using System;

namespace DeliveryService.Domain.Entities
{
    public class Route : BaseEntity
    {
        public string Name { get; protected set; }
        public Point Origin { get; protected set; }
        public Point Destination { get; protected set; }

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

        public void Update(Point origin, Point destination)
        {
            Origin = origin;
            Destination = destination;
            UpdatedAt = DateTime.UtcNow;
        }

        private string BuildName()
            => $"From {Origin.Name} To {Destination.Name}";

        internal bool ArePointsChanged(Point origin, Point destination)
        {
            return Origin.Equals(origin) is false || Destination.Equals(destination) is false;
        }

        internal bool IsTheSame(Route route) 
            => route.Origin.Equals(Origin) && route.Destination.Equals(Destination) && route.Active;
    }
}
