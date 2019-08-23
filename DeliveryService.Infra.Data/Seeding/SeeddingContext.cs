using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DeliveryService.Infra.Data.Seeding
{
    public class SeeddingContext
    {
        public static void Seed(IConfiguration configuration)
        {
            var context = new MongoContext(configuration);

            if (AlredySeeded(context)) return;

            CreateUsers(context);
            InsertSeedInfo(context);
        }

        public static void CreateUsers(MongoContext context)
        {

            var users = new List<User>
            {
                new User("Admin","admin@deliveryservice.com",Hash("admin"),UserRole.Admin),
                new User("User","user@deliveryservice.com",Hash("user"),UserRole.User),
            };

            context.Db.GetCollection<User>(MongoCollections.User).InsertMany(users);
        }

        private static bool AlredySeeded(MongoContext context) 
            => context.Db.GetCollection<object>("seedinfo").AsQueryable().Any();

        private static void InsertSeedInfo(MongoContext context) => context.Db.GetCollection<object>("seedinfo")
                .InsertOne(new { Id = ObjectId.GenerateNewId(), Active = true, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });

        private static string Hash(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);

            using (var sHa256Managed = new SHA256Managed())
            {
                var crypto = sHa256Managed.ComputeHash(data);
                var hash = new StringBuilder();

                for (var i = 0; i < crypto.Length; i++)
                {
                    hash.Append(crypto[i].ToString("x2"));
                }

                return hash.ToString();
            }
        }
    }
}
