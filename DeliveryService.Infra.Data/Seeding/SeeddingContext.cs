using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.ValueObject;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DeliveryService.Infra.Data.Seeding
{
    public class SeeddingContext
    {
        public static void Seed(IConfiguration configuration, IHostingEnvironment environment)
        {
            var context = new MongoContext(configuration);

            if (AlredySeeded(context)) return;

            CreateUsers(context);

            if(environment.IsEnvironment("Testing") is false)
            {
                CreatePoints(context);
                CreateConnections(context);
            }
            
            CreateSeedInfo(context);
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

        public static void CreatePoints(MongoContext context)
        {
            try
            {
                var points = BuildPoints();

                context.GetCollection<Point>(MongoCollections.Point).InsertMany(points);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateConnections(MongoContext context)
        {
            //var points = BuildPoints();

            //var connections = new List<Connection>
            //{
            //    new Connection(points.FirstOrDefault(x => x.Name == "A"), points.FirstOrDefault(x => x.Name == "C"),1,20),
            //    new Connection(points.FirstOrDefault(x => x.Name == "C"), points.FirstOrDefault(x => x.Name == "B"), 1,12),
            //    new Connection(points.FirstOrDefault(x => x.Name == "A"), points.FirstOrDefault(x => x.Name == "E"), 30,5),
            //    new Connection(points.FirstOrDefault(x => x.Name == "A"), points.FirstOrDefault(x => x.Name == "H"), 10,1),
            //    new Connection(points.FirstOrDefault(x => x.Name == "H"), points.FirstOrDefault(x => x.Name == "E"), 30,1),
            //    new Connection(points.FirstOrDefault(x => x.Name == "E"), points.FirstOrDefault(x => x.Name == "D"), 3,5),
            //    new Connection(points.FirstOrDefault(x => x.Name == "D"), points.FirstOrDefault(x => x.Name == "F"), 4,50),
            //    new Connection(points.FirstOrDefault(x => x.Name == "F"), points.FirstOrDefault(x => x.Name == "G"), 40,50),
            //    new Connection(points.FirstOrDefault(x => x.Name == "G"), points.FirstOrDefault(x => x.Name == "B"), 64,73),
            //    new Connection(points.FirstOrDefault(x => x.Name == "F"), points.FirstOrDefault(x => x.Name == "I"), 45,50),
            //    new Connection(points.FirstOrDefault(x => x.Name == "I"), points.FirstOrDefault(x => x.Name == "B"), 65,5),
            //};

            //context.GetCollection<Connection>(MongoCollections.Connection).InsertMany(connections);

        }

        public static void CreateRoutes(MongoContext context)
        {
            var points = BuildPoints();
            //var routes = new List<Route>
            //{
            //    new Route(points.FirstOrDefault(x => x.Name == "A"), points.FirstOrDefault(x => x.Name == "C"),1,20),
            //    new Route(points.FirstOrDefault(x => x.Name == "C"), points.FirstOrDefault(x => x.Name == "B"), 1,12),
            //    new Route(points.FirstOrDefault(x => x.Name == "A"), points.FirstOrDefault(x => x.Name == "E"), 30,5),
            //    new Route(points.FirstOrDefault(x => x.Name == "A"), points.FirstOrDefault(x => x.Name == "H"), 10,1),
            //    new Route(points.FirstOrDefault(x => x.Name == "H"), points.FirstOrDefault(x => x.Name == "E"), 30,1),
            //    new Route(points.FirstOrDefault(x => x.Name == "E"), points.FirstOrDefault(x => x.Name == "D"), 3,5),
            //    new Route(points.FirstOrDefault(x => x.Name == "D"), points.FirstOrDefault(x => x.Name == "F"), 4,50),
            //    new Route(points.FirstOrDefault(x => x.Name == "F"), points.FirstOrDefault(x => x.Name == "G"), 40,50),
            //    new Route(points.FirstOrDefault(x => x.Name == "G"), points.FirstOrDefault(x => x.Name == "B"), 64,73),
            //    new Route(points.FirstOrDefault(x => x.Name == "F"), points.FirstOrDefault(x => x.Name == "I"), 45,50),
            //    new Route(points.FirstOrDefault(x => x.Name == "I"), points.FirstOrDefault(x => x.Name == "B"), 65,5),
            //};

            var routes = new List<Route>
            {
                new Route(points.FirstOrDefault(x => x.Name == "A"), points.FirstOrDefault(x => x.Name == "C")),
                new Route(points.FirstOrDefault(x => x.Name == "C"), points.FirstOrDefault(x => x.Name == "B")),
                new Route(points.FirstOrDefault(x => x.Name == "A"), points.FirstOrDefault(x => x.Name == "E")),
                new Route(points.FirstOrDefault(x => x.Name == "A"), points.FirstOrDefault(x => x.Name == "H")),
                new Route(points.FirstOrDefault(x => x.Name == "H"), points.FirstOrDefault(x => x.Name == "E")),
                new Route(points.FirstOrDefault(x => x.Name == "E"), points.FirstOrDefault(x => x.Name == "D")),
                new Route(points.FirstOrDefault(x => x.Name == "D"), points.FirstOrDefault(x => x.Name == "F")),
                new Route(points.FirstOrDefault(x => x.Name == "F"), points.FirstOrDefault(x => x.Name == "G")),
                new Route(points.FirstOrDefault(x => x.Name == "G"), points.FirstOrDefault(x => x.Name == "B")),
                new Route(points.FirstOrDefault(x => x.Name == "F"), points.FirstOrDefault(x => x.Name == "I")),
                new Route(points.FirstOrDefault(x => x.Name == "I"), points.FirstOrDefault(x => x.Name == "B")),
            };

            context.GetCollection<Route>(MongoCollections.Route).InsertMany(routes);

        }

        private static bool AlredySeeded(MongoContext context)
            => context.Db.GetCollection<object>("seedinfo").AsQueryable().Any();

        private static void CreateSeedInfo(MongoContext context) => context.Db.GetCollection<object>("seedinfo")
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


        private static List<Point> BuildPoints()
            => new List<Point>
            {
                new Point("A"),
                new Point("B"),
                new Point("C"),
                new Point("D"),
                new Point("E"),
                new Point("F"),
                new Point("G"),
                new Point("H"),
                new Point("I")
            };
    }
}
