using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.Domain.Entities
{
    public abstract class BaseEntity
    {
        public ObjectId Id { get; protected set; }
        public bool Active { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
    }
}
