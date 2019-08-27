using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using MongoDB.Bson;
using System;

namespace DeliveryService.Domain.ValueObject
{
    public class Point : BaseEntity
    {
        public string Name { get; protected set; }

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

        public Point SetId(string id)
        {
            Id = ObjectId.Parse(id);
            return this;
        }

        internal static Point Create(CreatePointCommand command)
            => new Point(command.Name);

        internal void Update(UpdatePointCommand command)
        {
            Name = command.Name;
            UpdatedAt = DateTime.UtcNow;
        }

        public override bool Equals(object obj)
        {
            var valueObj = obj as Point;

            if (ReferenceEquals(valueObj, null)) return false;

            return EqualsCore(valueObj);
        }

        private bool EqualsCore(Point other)
            => other.Id == Id
                && other.Name == Name
                && other.Active == Active
                && other.CreatedAt == CreatedAt
                && other.UpdatedAt == UpdatedAt;

        public override int GetHashCode()
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

        internal bool IsTheSame(Point point)
            => point.Name == Name && point.Active;
    }
}
