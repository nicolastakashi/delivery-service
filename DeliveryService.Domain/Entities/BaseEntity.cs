using MongoDB.Bson;
using System;

namespace DeliveryService.Domain.Entities
{
    public abstract class BaseEntity
    {
        public ObjectId Id { get; protected set; }
        public bool Active { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        public void Inactive()
        {
            Active = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
