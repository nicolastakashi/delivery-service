using MongoDB.Bson;
using System;

namespace DeliveryService.Domain.Entities
{
    public abstract class BaseEntity
    {
        public ObjectId Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Inactive()
        {
            Active = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
