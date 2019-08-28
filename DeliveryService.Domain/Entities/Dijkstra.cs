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

        private Dijkstra(Point[] points)
        {
            MapPointToAdjacents(points);
        }

        public static Dijkstra Setup(Connection[] connections, Point[] points)
        {
            var Dijkstra = new Dijkstra(points);

            for (int index = 0; index < connections.Count(); index++)
            {
                var weight = connections[index].Cost * connections[index].Time;
                Dijkstra.AddEdge(connections[index].Origin.Id, connections[index].Destination.Id, weight);
            }

            return Dijkstra;
        }

        private void MapPointToAdjacents(Point[] points)
        {
            for (int index = 0; index < points.Count(); index++)
            {
                _adjacents.Add(points[index].Id, new List<Edge>());
            }
        }

        public int Count => _adjacents.Count;
        public bool HasEdge(ObjectId origin, ObjectId destination) => _adjacents[origin].Any(p => p.Id == destination);

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
                    bool shoulRemoveEdgeFault = ShoulRemoveEdgeFault(origin, originEdges, faultedEdges, node, edge.Id);

                    if (visited.ContainsKey(edge.Id) && shoulRemoveEdgeFault == false) continue;

                    var itinerary = itineraries[node];
                    var point = itineraries[edge.Id];

                    if (ShouldAddToFaultedList(originEdges, faultedEdges, edge.Id)) AddToFaultedList(faultedEdges, edge.Id);

                    if (shoulRemoveEdgeFault && isFirstRemove is false) isFirstRemove = true;
                    
                    decimal calculatedDistance = CalculateDistance(edge, itinerary);

                    if (point.ShoulUpdateDistanceAndPoin(calculatedDistance, isFirstRemove))
                    {
                        point.Update(node, calculatedDistance);
                        heap.Push((edge.Id, calculatedDistance));
                    }
                }

                visited[node] = true;
            }

            return Path(origin, destination, itineraries).Reverse();
        }

        private static decimal CalculateDistance(Edge edge, Itinerary itinerary) 
            => itinerary.Distance + edge.Weight;

        private static void AddToFaultedList(List<ObjectId> faultedEdges, ObjectId point) => faultedEdges.Add(point);

        private static bool ShouldAddToFaultedList(List<Edge> originEdges, List<ObjectId> faultedEdges, ObjectId point)
            => faultedEdges.Any(y => y == point) is false && originEdges.Any(x => x.Id == point);

        private static bool ShoulRemoveEdgeFault(ObjectId origin, List<Edge> originEdges, List<ObjectId> faultedEdges, ObjectId node, ObjectId point)
            => node != origin && faultedEdges.Any(x => x == point) && originEdges.Any(x => x.Id == point);

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
