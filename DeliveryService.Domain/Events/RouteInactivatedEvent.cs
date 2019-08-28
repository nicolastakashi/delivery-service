using DeliveryService.Domain.Entities;
using MediatR;

namespace DeliveryService.Domain.Events
{
    public class RouteInactivatedEvent : INotification
    {
        public Route Route { get; set; }

        public RouteInactivatedEvent(Route route)
        {
            Route = route;
        }
    }
}
