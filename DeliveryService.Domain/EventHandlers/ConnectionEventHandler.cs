using DeliveryService.Domain.Events;
using DeliveryService.Domain.Repositories.Write;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Domain.EventHandlers
{
    public class ConnectionEventHandler :
        INotificationHandler<ConnectionCreatedEvent>,
        INotificationHandler<ConnectionUpdatedEvent>,
        INotificationHandler<ConnectionInactivatedEvent>
    {
        private readonly IConnectionRepository _connectionRepository;
        private readonly IRouteRepository _routeRepository;

        public ConnectionEventHandler(IConnectionRepository connectionRepository, IRouteRepository routeRepository)
        {
            _connectionRepository = connectionRepository;
            _routeRepository = routeRepository;
        }

        public Task Handle(ConnectionInactivatedEvent notification, CancellationToken cancellationToken)
        {
            _connectionRepository.ClearCache();
            _routeRepository.ClearCache();

            return Task.CompletedTask;
        }

        public Task Handle(ConnectionUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _connectionRepository.ClearCache();
            _routeRepository.ClearCache();

            return Task.CompletedTask;
        }

        public Task Handle(ConnectionCreatedEvent notification, CancellationToken cancellationToken)
        {
            _connectionRepository.ClearCache();
            _routeRepository.ClearCache();

            return Task.CompletedTask;
        }
    }
}
