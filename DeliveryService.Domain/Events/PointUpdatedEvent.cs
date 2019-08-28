using DeliveryService.Domain.ValueObject;
using MediatR;

namespace DeliveryService.Domain.Events
{
    public class PointUpdatedEvent : INotification
    {
        public Point Point { get; set; }

        public PointUpdatedEvent(Point point)
        {
            Point = point;
        }
    }
}
