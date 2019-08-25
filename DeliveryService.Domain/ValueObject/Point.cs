using DeliveryService.Domain.Commands;
using MongoDB.Bson;
using System;

namespace DeliveryService.Domain.ValueObject
{
    public class Point : BaseValueObject<Point>
    {
        public string Name { get; protected set; }
        public ObjectId Id { get; protected set; }
        public bool Active { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        protected Point()
        {
        }

        public Point(string name)
        {
            Id = ObjectId.GenerateNewId();
            Name = name;
            Active = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        internal static Point Create(CreatePointCommand command)
            => new Point(command.Name);

        internal void Inactive()
        {
            Active = false;
            UpdatedAt = DateTime.UtcNow;
        }

        internal void Update(UpdatePointCommand command)
        {
            Name = command.Name;
            UpdatedAt = DateTime.UtcNow;
        }

        protected override bool EqualsCore(Point other)
            => other.Id == Id
                && other.Name == Name
                && other.Active == Active
                && other.CreatedAt == CreatedAt
                && other.UpdatedAt == UpdatedAt;

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Active.GetHashCode();
                hashCode = (hashCode * 397) ^ CreatedAt.GetHashCode();
                hashCode = (hashCode * 397) ^ UpdatedAt.GetHashCode();
                return hashCode;
            }
        }
    }
}
