using DeliveryService.Domain.Entities;
using DeliveryService.Domain.ValueObject;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace DeliveryService.Test.Unit.Entities
{
    public class GraphTests
    {
        private static List<Point> _points;

        public GraphTests()
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
        }

        [Fact]
        public void Graph_WithPoints_SetupSuccess()
        {
            var graph = new Dijkstra (_points.ToArray());

            graph.Count.Should().Be(_points.Count);
        }
    }
}
