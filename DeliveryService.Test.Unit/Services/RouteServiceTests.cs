using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Service;
using DeliveryService.Domain.ValueObject;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DeliveryService.Test.Unit.Services
{
    public class RouteServiceTests
    {
        private static List<Point> _points;
        private static List<Connection> _connections;

        public RouteServiceTests()
        {
            _points = new List<Point>
            {
                new Point("A").SetId("5d6441a042a56c173573234a"),
                new Point("B").SetId("5d6441aa55b37e484313f1ab"),
                new Point("C").SetId("5d6441b372cf4983e658d6fc"),
                new Point("D").SetId("5d6441bb5dbb299a46c616dd"),
                new Point("E").SetId("5d6441c7ca2ed7030a0f825e"),
                new Point("F").SetId("5d6441d0980b1eacc9dc6a2f"),
                new Point("G").SetId("5d6441d49f3fc53e1cff4180"),
                new Point("H").SetId("5d6441de3818b03b315f3a31"),
                new Point("I").SetId("5d6441ecdab0be906c200a72")
            };

            _connections = new List<Connection>
            {
                new Connection(_points.FirstOrDefault(x => x.Name == "A"), _points.FirstOrDefault(x => x.Name == "C"), 1, 20),
                new Connection(_points.FirstOrDefault(x => x.Name == "C"), _points.FirstOrDefault(x => x.Name == "B"), 1, 12),
                new Connection(_points.FirstOrDefault(x => x.Name == "A"), _points.FirstOrDefault(x => x.Name == "E"), 30, 5),
                new Connection(_points.FirstOrDefault(x => x.Name == "A"), _points.FirstOrDefault(x => x.Name == "H"), 10, 1),
                new Connection(_points.FirstOrDefault(x => x.Name == "H"), _points.FirstOrDefault(x => x.Name == "E"), 30, 1),
                new Connection(_points.FirstOrDefault(x => x.Name == "E"), _points.FirstOrDefault(x => x.Name == "D"), 3, 5),
                new Connection(_points.FirstOrDefault(x => x.Name == "D"), _points.FirstOrDefault(x => x.Name == "F"), 4, 50),
                new Connection(_points.FirstOrDefault(x => x.Name == "F"), _points.FirstOrDefault(x => x.Name == "G"), 40, 50),
                new Connection(_points.FirstOrDefault(x => x.Name == "G"), _points.FirstOrDefault(x => x.Name == "B"), 64, 73),
                new Connection(_points.FirstOrDefault(x => x.Name == "F"), _points.FirstOrDefault(x => x.Name == "I"), 45, 50),
                new Connection(_points.FirstOrDefault(x => x.Name == "I"), _points.FirstOrDefault(x => x.Name == "B"), 65, 5),
            };

            //_connections = new List<Connection>
            //{
            //    new Connection(_points.FirstOrDefault(x => x.Name == "A"), _points.FirstOrDefault(x => x.Name == "C"), 1, 20),
            //    new Connection(_points.FirstOrDefault(x => x.Name == "C"), _points.FirstOrDefault(x => x.Name == "B"), 1, 12),
            //    new Connection(_points.FirstOrDefault(x => x.Name == "C"), _points.FirstOrDefault(x => x.Name == "E"), 1, 1),
            //    new Connection(_points.FirstOrDefault(x => x.Name == "A"), _points.FirstOrDefault(x => x.Name == "E"), 30, 1),
            //    new Connection(_points.FirstOrDefault(x => x.Name == "A"), _points.FirstOrDefault(x => x.Name == "H"), 10, 1),
            //    new Connection(_points.FirstOrDefault(x => x.Name == "H"), _points.FirstOrDefault(x => x.Name == "E"), 30, 52),
            //    new Connection(_points.FirstOrDefault(x => x.Name == "E"), _points.FirstOrDefault(x => x.Name == "D"), 3, 5),
            //    new Connection(_points.FirstOrDefault(x => x.Name == "D"), _points.FirstOrDefault(x => x.Name == "F"), 4, 50),
            //    new Connection(_points.FirstOrDefault(x => x.Name == "F"), _points.FirstOrDefault(x => x.Name == "G"), 40, 50),
            //    new Connection(_points.FirstOrDefault(x => x.Name == "G"), _points.FirstOrDefault(x => x.Name == "B"), 64, 73),
            //    new Connection(_points.FirstOrDefault(x => x.Name == "F"), _points.FirstOrDefault(x => x.Name == "I"), 45, 50),
            //    new Connection(_points.FirstOrDefault(x => x.Name == "I"), _points.FirstOrDefault(x => x.Name == "B"), 65, 5),
            //};
        }

        [Fact]
        public void TestSetupGraph()
        {
            var routeService = new RouteService(UnitOfMeasure.Cost, _connections.ToArray(), _points.ToArray());
        }

        [Fact]
        public void TestFindBestPath()
        {
            var routeService = new RouteService(UnitOfMeasure.Cost, _connections.ToArray(), _points.ToArray());

            var origin = _points.FirstOrDefault(x => x.Name == "D");
            var destination = _points.FirstOrDefault(x => x.Name == "E");

            routeService.FindBestPath(origin, destination);
        }
    }
}
