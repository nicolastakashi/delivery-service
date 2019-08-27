using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.ValueObject;
using System.Linq;

namespace DeliveryService.Domain.Service
{
    public sealed class RouteService
    {
        private readonly Dijkstra  _graph;
        private readonly Point[] _points;

        public RouteService(UnitOfMeasure unitOfMeasure, Connection[] connections, Point[] points)
        {
            _graph = GraphSetup(unitOfMeasure, connections, points);
            _points = points;
        }

        public DomainResult<BestRoute> FindBestPath(Point origin, Point destination)
        {
            var path = _graph.FindBestPath(origin.Id, destination.Id);
            var wayPointsId = path.Where(x => x.Point != origin.Id && x.Point != destination.Id).Select(x => x.Point);

            if (wayPointsId.Any() is false)
            {
                return DomainResult.Failure<BestRoute>("Theres is no exist a valid path for this route");
            }

            var wayPoints = _points.Where(x => wayPointsId.Contains(x.Id));
            var weight = path.Last().Distance;

            var result = new BestRoute(origin, destination, wayPoints, weight, UnitOfMeasure.Cost);

            return DomainResult.Ok(result);
        }

        internal Dijkstra  GraphSetup(UnitOfMeasure unitOfMeasure, Connection[] connections, Point[] points)
        {
            var graph = new Dijkstra (points);

            for (int index = 0; index < connections.Count(); index++)
            {
                var weight = unitOfMeasure == UnitOfMeasure.Cost ? connections[index].Cost : connections[index].Time;
                graph.AddEdge(connections[index].Origin.Id, connections[index].Destination.Id, weight);
            }

            return graph;
        }
    }
}
