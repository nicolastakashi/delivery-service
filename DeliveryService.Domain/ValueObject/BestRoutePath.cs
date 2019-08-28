using DeliveryService.Domain.Enums;
using MongoDB.Bson;
using System.Collections.Generic;

namespace DeliveryService.Domain.ValueObject
{
    public class BestRoutePath
    {
        public ObjectId RouteId { get; set; }
        public Point Origin { get; set; }
        public Point Destination { get; set; }
        public IEnumerable<Point> WayPoints { get; set; }
        public decimal Weight { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public BestRoutePath(ObjectId routeId, Point origin, Point destination, IEnumerable<Point> wayPoints, decimal weight, UnitOfMeasure unitOfMeasure)
        {
            RouteId = routeId;
            Origin = origin;
            Destination = destination;
            WayPoints = wayPoints;
            Weight = weight;
            UnitOfMeasure = unitOfMeasure;
        }
    }
}
