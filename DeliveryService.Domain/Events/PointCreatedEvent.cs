using DeliveryService.Domain.ValueObject;
using MediatR;

namespace DeliveryService.Domain.Events
{
    public class PointCreatedEvent : INotification
    {
        public Point Point { get; set; }

        public PointCreatedEvent(Point point)
        {
            Point = point;
        }
    }
}
