using DeliveryService.Domain.Enums;
using MongoDB.Bson;
using System;

namespace DeliveryService.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public UserRole Role { get; set; }

        public User(string name, string email, string password, UserRole role)
        {
            Id = ObjectId.GenerateNewId();
            Name = name;
            Email = email;
            Password = password;
            Role = role;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Active = true;
        }

        protected User()
        {
        }
    }
}
