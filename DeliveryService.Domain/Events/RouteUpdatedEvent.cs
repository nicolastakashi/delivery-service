using DeliveryService.Domain.Entities;
using MediatR;

namespace DeliveryService.Domain.Events
{
    public class RouteUpdatedEvent : INotification
    {
        public Route Route { get; set; }

        public RouteUpdatedEvent(Route route)
        {
            Route = route;
        }
    }
}
