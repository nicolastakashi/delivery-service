using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Domain.Entities
{
    internal sealed class Graph
    {
        private readonly Dictionary<ObjectId, List<Edge>> adjacency = new Dictionary<ObjectId, List<Edge>>();

        public Graph(Point[] points)
        {
            for (int index = 0; index < points.Count(); index++)
            {
                adjacency.Add(points[index].Id, new List<Edge>());
            }
        }

        public int Count => adjacency.Count;
        public bool HasEdge(ObjectId origin, ObjectId destination) => adjacency[origin].Any(p => p.Node == destination);


        public bool AddEdge(ObjectId origin, ObjectId destination, decimal weight)
        {
            if (HasEdge(origin, destination)) return false;

            adjacency[origin].Add(new Edge(destination, weight));

            return true;
        }

        public Itinerary[] FindPath(ObjectId origin)
        {
            var itineraries = adjacency.Select(i => new Itinerary { Distance = decimal.MaxValue, Previous = i.Key }).ToArray();

            itineraries.FirstOrDefault(x => x.Previous == origin).Distance = 0;

            var visited = new Dictionary<ObjectId, bool>();

            var heap = new Heap<(ObjectId node, decimal distance)>((a, b) => a.distance.CompareTo(b.distance));

            heap.Push((origin, 0));

            while (heap.Count > 0)
            {
                var current = heap.Pop();

                if (visited.ContainsKey(current.node)) continue;

                var edges = adjacency[current.node];

                foreach (var edge in edges)
                {
                    var point = edge.Node;

                    if (visited.ContainsKey(point)) continue;

                    var itinerary = itineraries.FirstOrDefault(x => x.Previous == current.node);
                    var itineraryPoint = itineraries.FirstOrDefault(x => x.Previous == point);

                    decimal calcDistance = itinerary.Distance + edge.Weight;

                    if (calcDistance < itineraryPoint.Distance)
                    {
                        itineraryPoint.Previous = current.node;
                        itineraryPoint.Distance = calcDistance;
                        heap.Push((point, calcDistance));
                    }
                }

                visited[current.node] = true;
            }
            return itineraries;
        }
    }

    internal sealed class Itinerary
    {
        public decimal Distance { get; set; }
        public ObjectId Previous { get; set; }
    }

    internal sealed class Edge
    {
        public ObjectId Node { get; private set; }
        public decimal Weight { get; private set; }

        public Edge()
        {
        }

        public Edge(ObjectId node, decimal weight)
        {
            Node = node;
            Weight = weight;
        }
    }

    internal sealed class EdgeList
    {
        public Dictionary<ObjectId, List<Edge>> Edges { get; set; }
    }

    internal sealed class Heap<T>
    {
        private readonly IComparer<T> comparer;
        private readonly List<T> list = new List<T> { default };

        public Heap() : this(default(IComparer<T>)) { }

        public Heap(IComparer<T> comparer)
        {
            this.comparer = comparer ?? Comparer<T>.Default;
        }

        public Heap(Comparison<T> comparison) : this(Comparer<T>.Create(comparison)) { }

        public int Count => list.Count - 1;

        public void Push(T element)
        {
            list.Add(element);
            SiftUp(list.Count - 1);
        }

        public T Pop()
        {
            T result = list[1];
            list[1] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            SiftDown(1);
            return result;
        }

        private static int Parent(int i) => i / 2;
        private static int Left(int i) => i * 2;
        private static int Right(int i) => i * 2 + 1;

        private void SiftUp(int i)
        {
            while (i > 1)
            {
                int parent = Parent(i);
                if (comparer.Compare(list[i], list[parent]) > 0) return;
                (list[parent], list[i]) = (list[i], list[parent]);
                i = parent;
            }
        }

        private void SiftDown(int i)
        {
            for (int left = Left(i); left < list.Count; left = Left(i))
            {
                int smallest = comparer.Compare(list[left], list[i]) <= 0 ? left : i;
                int right = Right(i);
                if (right < list.Count && comparer.Compare(list[right], list[smallest]) <= 0) smallest = right;
                if (smallest == i) return;
                (list[i], list[smallest]) = (list[smallest], list[i]);
                i = smallest;
            }
        }
    }
}
