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
        private const decimal MAX = decimal.MaxValue;

        private Dijkstra(Point[] points)
        {
            MapPointToAdjacents(points);
        }

        public static Dijkstra Setup(Connection[] connections, Point[] points)
        {
            var dijkstra = new Dijkstra(points);

            for (int index = 0; index < connections.Count(); index++)
            {
                var weight = connections[index].Cost * connections[index].Time;
                dijkstra.AddEdge(connections[index].Origin.Id, connections[index].Destination.Id, weight);
            }

            return dijkstra;
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
            var visitedNodes = new Dictionary<ObjectId, bool>();

            itineraries[origin].Distance = 0;

            var heap = new Heap<Itinerary>(new Itinerary(origin, 0), (a, b) => a.Distance.CompareTo(b.Distance));
            heap.Push(new Itinerary(origin, 0));
            
            Edge edge;
            Itinerary node;
            Itinerary point;
            List<Edge> edges;
            Itinerary itinerary;
            List<ObjectId> faultedEdges = new List<ObjectId>(_adjacents.Count);
            bool wasFaultRemoved = false;

            while (heap.Count > 0)
            {
                node = heap.Pop();

                if (visitedNodes.ContainsKey(node.Point)) continue;

                edges = _adjacents[node.Point];

                for (int i = 0; i < edges.Count; i++)
                {
                    edge = edges[i];
                    bool shoulRemoveEdgeFault = ShouldRemoveEdgeFault(origin, _adjacents[origin], faultedEdges, node.Point, edge.Id);

                    if (visitedNodes.ContainsKey(edge.Id) && shoulRemoveEdgeFault == false) continue;

                    itinerary = itineraries[node.Point];
                    point = itineraries[edge.Id];

                    if (ShouldAddToFaultedList(_adjacents[origin], faultedEdges, edge.Id)) AddToFaultedList(faultedEdges, edge.Id);

                    if (shoulRemoveEdgeFault && wasFaultRemoved is false) wasFaultRemoved = true;

                    decimal calculatedDistance = CalculateDistance(edge, itinerary);

                    if (point.ShouldUpdateDistanceAndPoint(calculatedDistance, wasFaultRemoved && visitedNodes.ContainsKey(edge.Id)))
                    {
                        point.Update(node.Point, calculatedDistance);
                        heap.Push(new Itinerary(edge.Id, calculatedDistance));
                    }
                }

                visitedNodes[node.Point] = true;
            }

            return Path(origin, destination, itineraries).Reverse();
        }

        private static decimal CalculateDistance(Edge edge, Itinerary itinerary)
            => itinerary.Distance + edge.Weight;

        private static void AddToFaultedList(List<ObjectId> faultedEdges, ObjectId point) => faultedEdges.Add(point);

        private static bool ShouldAddToFaultedList(List<Edge> originEdges, List<ObjectId> faultedEdges, ObjectId point)
            => faultedEdges.Any(y => y == point) is false && originEdges.Any(x => x.Id == point);

        private static bool ShouldRemoveEdgeFault(ObjectId origin, List<Edge> originEdges, List<ObjectId> faultedEdges, ObjectId node, ObjectId point)
            => node != origin && faultedEdges.Any(x => x == point) && originEdges.Any(x => x.Id == point);

        public IEnumerable<Itinerary> Path(ObjectId start, ObjectId end, Dictionary<ObjectId, Itinerary> itineraries)
        {
            var result = new List<Itinerary>(itineraries.Count);

            if (end == itineraries[end].Point) return result;

            result.Add(new Itinerary(end, itineraries[end].Distance));

            for (var i = end; i != start; i = itineraries[i].Point)
            {
                result.Add(new Itinerary(itineraries[i].Point, itineraries[itineraries[i].Point].Distance));
            }

            if (result.Count < 3) result.RemoveAll(x => true);

            return result;
        }

        private Dictionary<ObjectId, Itinerary> BuildEstimatedItineraries()
        {
            var itineraries = new Dictionary<ObjectId, Itinerary>();

            foreach (var item in _adjacents)
            {
                itineraries.Add(item.Key, new Itinerary(item.Key, MAX));
            }

            return itineraries;
        }
    }
}
