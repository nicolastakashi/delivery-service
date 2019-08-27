using DeliveryService.Domain.ValueObject;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Domain.Entities
{
    public sealed class Dijkstra 
    {
        private readonly Dictionary<ObjectId, List<Edge>> _adjacents = new Dictionary<ObjectId, List<Edge>>();
        private const decimal MAX = decimal.MaxValue / 2;

        public Dijkstra (Point[] points)
        {
            MapPointToAdjacents(points);
        }

        private void MapPointToAdjacents(Point[] points)
        {
            for (int index = 0; index < points.Count(); index++)
            {
                _adjacents.Add(points[index].Id, new List<Edge>());
            }
        }

        public int Count => _adjacents.Count;
        public bool HasEdge(ObjectId origin, ObjectId destination) => _adjacents[origin].Any(p => p.Node == destination);

        public bool AddEdge(ObjectId origin, ObjectId destination, decimal weight)
        {
            if (HasEdge(origin, destination)) return false;

            _adjacents[origin].Add(new Edge(destination, weight));

            return true;
        }

        public IEnumerable<Itinerary> FindBestPath(ObjectId origin, ObjectId destination)
        {
            var itineraries = BuildEstimatedItineraries();
            var visited = new Dictionary<ObjectId, bool>();

            itineraries[origin].Distance = 0;

            var heap = new Heap<(ObjectId node, decimal distance)>((origin, 0), (a, b) => a.distance.CompareTo(b.distance));
            heap.Push((origin, 0));

            var originEdges = _adjacents[origin];
            var faultedEdges = new List<ObjectId>(_adjacents.Count);
            var isFirstRemove = false;

            while (heap.Count > 0)
            {
                var (node, distance) = heap.Pop();

                if (visited.ContainsKey(node)) continue;

                var edges = _adjacents[node];

                foreach (var edge in edges)
                {
                    var point = edge.Node;

                    var shouldRemoveFaulted = node != origin && faultedEdges.Any(x => x == point) && originEdges.Any(x => x.Node == point);

                    if (visited.ContainsKey(point) && shouldRemoveFaulted is false) continue;

                    var itinerary = itineraries[node];
                    var itineraryPoint = itineraries[point];

                    if (faultedEdges.Any(y => y == point) is false && originEdges.Any(x => x.Node == point)) faultedEdges.Add(point);


                    if (shouldRemoveFaulted && isFirstRemove is false)
                    {
                        isFirstRemove = true;
                    }

                    decimal calcDistance = itinerary.Distance + edge.Weight;

                    if (calcDistance < itineraryPoint.Distance || isFirstRemove)
                    {
                        itineraryPoint.Point = node;
                        itineraryPoint.Distance = calcDistance;

                        heap.Push((point, calcDistance));
                    }
                }

                visited[node] = true;
            }

            return Path(origin, destination, itineraries).Reverse();
        }

        public IEnumerable<Itinerary> Path(ObjectId start, ObjectId end, Dictionary<ObjectId, Itinerary> itineraries)
        {
            yield return new Itinerary { Distance = itineraries[end].Distance, Point = end };

            for (var i = end; i != start; i = itineraries[i].Point)
            {
                yield return new Itinerary { Distance = itineraries[itineraries[i].Point].Distance, Point = itineraries[i].Point };
            }
        }

        private Dictionary<ObjectId, Itinerary> BuildEstimatedItineraries()
        {
            var itineraries = new Dictionary<ObjectId, Itinerary>();

            foreach (var item in _adjacents)
            {
                itineraries.Add(item.Key, new Itinerary { Distance = MAX, Point = item.Key });
            }

            return itineraries;
        }
    }
}
