using DeliveryService.Domain.ValueObject;
using MediatR;

namespace DeliveryService.Domain.Events
{
    public class PointInactivatedEvent : INotification
    {
        public Point Point { get; set; }

        public PointInactivatedEvent(Point point)
        {
            Point = point;
        }
    }
}
