using DeliveryService.Domain.Commands;
using MongoDB.Bson;
using System;

namespace DeliveryService.Domain.Entities
{
    public class Point : BaseEntity
    {
        public string Name { get; protected set; }

        protected Point()
        {
        }

        protected Point(string name)
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
    }
}
