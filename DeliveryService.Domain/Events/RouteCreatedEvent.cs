using DeliveryService.Domain.Entities;
using MediatR;

namespace DeliveryService.Domain.Events
{
    public class RouteCreatedEvent : INotification
    {
        public Route Route { get; set; }

        public RouteCreatedEvent(Route route)
        {
            Route = route;
        }
    }
}
