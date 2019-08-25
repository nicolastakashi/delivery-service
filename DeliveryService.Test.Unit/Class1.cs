using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Service;
using DeliveryService.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DeliveryService.Test.Unit
{
    public class Class1
    {
        private static List<Point> _points;
        private static List<Connection> _connections;

        public Class1()
        {
            _points = new List<Point>
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
            var origin = _points.FirstOrDefault(x => x.Name == "A");
            var destination = _points.FirstOrDefault(x => x.Name == "B");

            routeService.FindBestPath(origin, destination);
        }
    }
}
