using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.ValueObject;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Domain.Service
{
    public sealed class RouteService
    {
        private readonly Dijkstra _graph;
        private readonly Point[] _points;
        private readonly UnitOfMeasure _unitOfMeasure;

        public RouteService(UnitOfMeasure unitOfMeasure, Connection[] connections, Point[] points)
        {
            _graph = DijkstraSetup(unitOfMeasure, connections, points);
            _points = points;
            _unitOfMeasure = unitOfMeasure;
        }

        public DomainResult<BestRoutePath> FindBestPath(Route route)
        {
            var path = _graph.FindBestPath(route.Origin.Id, route.Destination.Id);
            var wayPoints = FindWayPoints(path, route.Origin, route.Destination);

            if (wayPoints.Any() is false)
            {
                return DomainResult.Failure<BestRoutePath>("Theres is no exist a valid path for this route");
            }

            var weight = path.Last().Distance;
            var result = new BestRoutePath(route.Id, route.Origin, route.Destination, wayPoints, weight, _unitOfMeasure);

            return DomainResult.Ok(result);
        }

        private IEnumerable<Point> FindWayPoints(IEnumerable<Itinerary> bestPath, Point origin, Point destination)
        {
            var wayPointsId = bestPath.Where(x => x.Point != origin.Id && x.Point != destination.Id).Select(x => x.Point);
            return _points.Where(x => wayPointsId.Contains(x.Id));
        }

        private Dijkstra DijkstraSetup(UnitOfMeasure unitOfMeasure, Connection[] connections, Point[] points)
        {
            var Dijkstra = new Dijkstra(points);

            for (int index = 0; index < connections.Count(); index++)
            {
                var weight = connections[index].Cost * connections[index].Time;
                Dijkstra.AddEdge(connections[index].Origin.Id, connections[index].Destination.Id, weight);
            }

            return Dijkstra;
        }
    }
}
