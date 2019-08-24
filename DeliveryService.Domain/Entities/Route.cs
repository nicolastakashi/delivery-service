using DeliveryService.Domain.Commands;
using MongoDB.Bson;
using System;

namespace DeliveryService.Domain.Entities
{
    public class Route : BaseEntity
    {
        public string Name { get; protected set; }
        public Point Origin { get; protected set; }
        public Point Destination { get; protected set; }
        public int Time { get; protected set; }
        public int Cost { get; protected set; }

        protected Route()
        {
        }

        public Route(Point origin, Point destination, int time, int cost)
        {
            Id = ObjectId.GenerateNewId();
            Origin = origin;
            Destination = destination;
            Name = BuildName();
            Time = time;
            Cost = cost;
            Active = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static Route Create(CreateRouteCommand command, Point origin, Point destination)
            => new Route(origin, destination, command.Time, command.Cost);

        private string BuildName()
            => $"From {Origin.Name} To {Destination.Name}";
    }
}
