using DeliveryService.Domain.Entities;
using DeliveryService.Domain.ValueObject;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DeliveryService.Test.Unit.Entities
{
    public class DijkstraTests
    {
        private static List<Point> _points;
        private static List<Connection> _connections;
        public DijkstraTests()
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
        public void Dijkstra_WithPoints_SetupSuccess()
        {
            var dijkstra = new Dijkstra(_points.ToArray());

            dijkstra.Count.Should().Be(_points.Count);
        }

        [Fact]
        public void Dijkstra_Find_BestPath_BetWeen_A_And_E_Success()
        {

        }
    }
}
