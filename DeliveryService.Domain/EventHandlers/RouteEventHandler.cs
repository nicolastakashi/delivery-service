using DeliveryService.Domain.Events;
using DeliveryService.Domain.Repositories.Write;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Domain.EventHandlers
{
    public class RouteEventHandler :
        INotificationHandler<RouteCreatedEvent>,
        INotificationHandler<RouteUpdatedEvent>,
        INotificationHandler<RouteInactivatedEvent>
    {
        private IRouteRepository _routeRepository;

        public RouteEventHandler(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public Task Handle(RouteInactivatedEvent notification, CancellationToken cancellationToken)
        {
            _routeRepository.ClearCache();

            return Task.CompletedTask;
        }

        public Task Handle(RouteUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _routeRepository.ClearCache();

            return Task.CompletedTask;
        }

        public Task Handle(RouteCreatedEvent notification, CancellationToken cancellationToken)
        {
            _routeRepository.ClearCache();

            return Task.CompletedTask;
        }
    }
}
