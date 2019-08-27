using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using System.Collections.Generic;

namespace DeliveryService.Domain.ValueObject
{
    public class BestRoute : BaseEntity
    {
        public Point Origin { get; set; }
        public Point Destination { get; set; }
        public IEnumerable<Point> WayPoints { get; set; }
        public decimal Weight { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public BestRoute(Point origin, Point destination, IEnumerable<Point> wayPoints, decimal weight, UnitOfMeasure unitOfMeasure)
        {
            Origin = origin;
            Destination = destination;
            WayPoints = wayPoints;
            Weight = weight;
            UnitOfMeasure = unitOfMeasure;
        }
    }
}
