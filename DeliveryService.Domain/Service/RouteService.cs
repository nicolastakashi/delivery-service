using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Domain.Service
{
    public sealed class RouteService
    {
        private readonly Graph _graph;

        public RouteService(UnitOfMeasure unitOfMeasure, Connection[] connections, Point[] points)
        {
            _graph = GraphSetup(unitOfMeasure, connections, points);
        }

        public void FindBestPath(Point origin, Point destination)
        {
            var path = _graph.FindPath(origin.Id);
        }

        internal Graph GraphSetup(UnitOfMeasure unitOfMeasure, Connection[] connections, Point[] points)
        {
            var graph = new Graph(points);

            for (int index = 0; index < connections.Count(); index++)
            {
                var connection = connections[index];
                var origin = connection.Origin;
                var destination = connection.Destination;
                var weight = unitOfMeasure == UnitOfMeasure.Cost ? connection.Cost : connection.Time;

                graph.AddEdge(origin.Id, destination.Id, weight);
            }

            return graph;
        }
    }
}
