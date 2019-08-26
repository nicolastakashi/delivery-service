using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.ValueObject;
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
                var weight = unitOfMeasure == UnitOfMeasure.Cost ? connections[index].Cost : connections[index].Time;
                graph.AddEdge(connections[index].Origin.Id, connections[index].Destination.Id, weight);
            }

            return graph;
        }
    }
}
