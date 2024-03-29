﻿using DeliveryService.Domain.Entities;
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
            CreatePoints(context);

            if (environment.IsEnvironment("Testing"))
            {
                CreateTestConnections(context);
            }
            else
            {
                CreateConnections(context);
                CreateRoutes(context);
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
            var connections = new List<Connection>
            {
                CreateConnection("A", "C",1,20, "5d651266d6fb92d4e881694d"),
                CreateConnection("C", "B", 1,12,"5d65126ac6676c19fdf9d4c9"),
                CreateConnection("A", "E", 30,5,"5d65127055f62a64608c55a8"),
                CreateConnection("A", "H", 10,1,"5d6512732127ca5f44d62e63"),
                CreateConnection("H", "E", 30,1,"5d65127971a59f5dab47ed5b"),
                CreateConnection("E", "D", 3,5,"5d6512815b90904408c9a1b3"),
                CreateConnection("D", "F", 4,50,"5d6512850c8409a79e6d1b67"),
                CreateConnection("F", "G", 40,50,"5d65128b557c5f569ec05db4"),
                CreateConnection("G", "B", 64,73,"5d65129210148331c637c83a"),
                CreateConnection("F", "I", 45,50,"5d6512972934f26dfa188159"),
                CreateConnection("I", "B", 65,5,"5d65129ed4ef53e206b384ae"),
            };

            context.GetCollection<Connection>(MongoCollections.Connection).InsertMany(connections);
        }

        public static void CreateTestConnections(MongoContext context)
        {
            var connections = new List<Connection>
            {
                CreateConnection("A", "C",1,20, "5d651266d6fb92d4e881694d"),
                CreateConnection("C", "B", 1,12,"5d65126ac6676c19fdf9d4c9"),
            };

            context.GetCollection<Connection>(MongoCollections.Connection).InsertMany(connections);
        }

        public static void CreateRoutes(MongoContext context)
        {
            var points = BuildPoints();

            var routes = new List<Route>
            {
                new Route(points.FirstOrDefault(x => x.Name == "A"), points.FirstOrDefault(x => x.Name == "E")),
                new Route(points.FirstOrDefault(x => x.Name == "A"), points.FirstOrDefault(x => x.Name == "B")),
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
                BuildPoint("A","5d6441a042a56c173573234a"),
                BuildPoint("B","5d6441aa55b37e484313f1ab"),
                BuildPoint("C","5d6441b372cf4983e658d6fc"),
                BuildPoint("D","5d6441bb5dbb299a46c616dd"),
                BuildPoint("E","5d6441c7ca2ed7030a0f825e"),
                BuildPoint("F","5d6441d0980b1eacc9dc6a2f"),
                BuildPoint("G","5d6441d49f3fc53e1cff4180"),
                BuildPoint("H","5d6441de3818b03b315f3a31"),
                BuildPoint("I","5d6441ecdab0be906c200a72")
            };

        private static Point BuildPoint(string name, string id)
        {
            var point = new Point(name)
            {
                Id = ObjectId.Parse(id)
            };
            return point;
        }

        private static Connection CreateConnection(string origin, string destination, decimal time, decimal cost, string id)
        {
            var points = BuildPoints();

            return new Connection(points.FirstOrDefault(x => x.Name == origin), points.FirstOrDefault(x => x.Name == destination), time, cost)
            {
                Id = ObjectId.Parse(id)
            };
        }
    }
}
