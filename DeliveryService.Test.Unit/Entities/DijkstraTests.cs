using DeliveryService.Domain.Entities;
using DeliveryService.Domain.ValueObject;
using FluentAssertions;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DeliveryService.Test.Unit.Entities
{
    public class DijkstraTests
    {
        private static List<Point> _points;
        private static List<Connection> _connections;
        private readonly Dijkstra _dijkstra;

        public DijkstraTests()
        {
            _points = BuildPoints();

            _connections = new List<Connection>
            {
                new Connection(_points.FirstOrDefault(x => x.Name == "A"), _points.FirstOrDefault(x => x.Name == "C"), 1, 20), // 20
                new Connection(_points.FirstOrDefault(x => x.Name == "E"), _points.FirstOrDefault(x => x.Name == "D"), 3, 5), // 15
                new Connection(_points.FirstOrDefault(x => x.Name == "C"), _points.FirstOrDefault(x => x.Name == "E"), 10, 50), // 500
                new Connection(_points.FirstOrDefault(x => x.Name == "C"), _points.FirstOrDefault(x => x.Name == "B"), 1, 12), // 12
                new Connection(_points.FirstOrDefault(x => x.Name == "A"), _points.FirstOrDefault(x => x.Name == "E"), 30, 5), // 150
                new Connection(_points.FirstOrDefault(x => x.Name == "A"), _points.FirstOrDefault(x => x.Name == "H"), 10, 1), // 10
                new Connection(_points.FirstOrDefault(x => x.Name == "H"), _points.FirstOrDefault(x => x.Name == "E"), 30, 1), // 30
                new Connection(_points.FirstOrDefault(x => x.Name == "D"), _points.FirstOrDefault(x => x.Name == "F"), 4, 50), // 200
                new Connection(_points.FirstOrDefault(x => x.Name == "F"), _points.FirstOrDefault(x => x.Name == "G"), 40, 50), // 2000
                new Connection(_points.FirstOrDefault(x => x.Name == "G"), _points.FirstOrDefault(x => x.Name == "B"), 64, 73), //4672
                new Connection(_points.FirstOrDefault(x => x.Name == "F"), _points.FirstOrDefault(x => x.Name == "I"), 45, 50), // 2250
                new Connection(_points.FirstOrDefault(x => x.Name == "I"), _points.FirstOrDefault(x => x.Name == "B"), 65, 5), //325
                new Connection(_points.FirstOrDefault(x => x.Name == "A"), _points.FirstOrDefault(x => x.Name == "J"), 1, 5), //325
                new Connection(_points.FirstOrDefault(x => x.Name == "J"), _points.FirstOrDefault(x => x.Name == "E"), 10, 100), //325
            };

            _dijkstra = Dijkstra.Setup(_connections.ToArray(), _points.ToArray());
        }

        [Fact]
        public void Dijkstra_WithPoints_SetupSuccess()
        {
            _dijkstra.Count.Should().Be(_points.Count);
        }

        [Fact]
        public void Dijkstra_Find_BestPath_BetWeen_A_And_E_Success()
        {
            // Arrange
            var origin = _points.First(x => x.Name == "A");
            var destination = _points.First(x => x.Name == "E");
            var wayPoint = _points.First(x => x.Name == "H");   

            //Act
            var result = _dijkstra.FindBestPath(origin.Id, destination.Id);

            // Assert
            result.Should().HaveCount(3);
            result.FirstOrDefault(x => x.Point == wayPoint.Id).Should().NotBeNull();
        }

        [Fact]
        public void Dijkstra_Find_BestPath_BetWeen_A_And_B_Success()
        {
            // Arrange
            var origin = _points.First(x => x.Name == "A");
            var destination = _points.First(x => x.Name == "B");
            var wayPoint = _points.First(x => x.Name == "C");
            //Act
            var result = _dijkstra.FindBestPath(origin.Id, destination.Id);

            // Assert
            result.Should().HaveCount(3);
            result.FirstOrDefault(x => x.Point == wayPoint.Id).Should().NotBeNull();
        }

        [Fact]
        public void Dijkstra_Find_BestPath_BetWeen_E_And_B_Success()
        {
            // Arrange
            var wayPointsName = new string[] { "D", "F", "I" };

            var origin = _points.First(x => x.Name == "E");
            var destination = _points.First(x => x.Name == "B");
            var wayPoints = _points.Where(x => wayPointsName.Contains(x.Name));

            //Act
            var result = _dijkstra.FindBestPath(origin.Id, destination.Id);

            // Assert
            result.Should().HaveCount(5);
            result.Count(x => wayPoints.Any(y => y.Id == x.Point)).Should().Be(3);
        }


        [Fact]
        public void Dijkstra_Find_BestPath_BetWeen_H_And_A_Success()
        {
            // Arrange
            var origin = _points.First(x => x.Name == "H");
            var destination = _points.First(x => x.Name == "A");

            //Act
            var result = _dijkstra.FindBestPath(origin.Id, destination.Id);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Dijkstra_Find_BestPath_BetWeen_A_And_H_Success()
        {
            // Arrange
            var origin = _points.First(x => x.Name == "A");
            var destination = _points.First(x => x.Name == "H");

            //Act
            var result = _dijkstra.FindBestPath(origin.Id, destination.Id);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Dijkstra_Find_BestPath_BetWeen_C_And_H_Success()
        {
            // Arrange
            var origin = _points.First(x => x.Name == "C");
            var destination = _points.First(x => x.Name == "H");

            //Act
            var result = _dijkstra.FindBestPath(origin.Id, destination.Id);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Dijkstra_Find_BestPath_BetWeen_C_And_D_Success()
        {
            // Arrange
            var origin = _points.First(x => x.Name == "C");
            var destination = _points.First(x => x.Name == "D");
            var wayPoint = _points.First(x => x.Name == "E");

            //Act
            var result = _dijkstra.FindBestPath(origin.Id, destination.Id);

            // Assert
            result.Should().HaveCount(3);
            result.FirstOrDefault(x => x.Point == wayPoint.Id).Should().NotBeNull();
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
                BuildPoint("I","5d6441ecdab0be906c200a72"),
                BuildPoint("J","5d6441ecdab0be906c200a73")
            };

        private static Point BuildPoint(string name, string id)
        {
            var point = new Point(name)
            {
                Id = ObjectId.Parse(id)
            };
            return point;
        }
    }
}
