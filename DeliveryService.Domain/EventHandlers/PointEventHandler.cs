using DeliveryService.Domain.Events;
using DeliveryService.Domain.Repositories.Write;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Domain.EventHandlers
{
    public class PointEventHandler : 
        INotificationHandler<PointUpdatedEvent>,
        INotificationHandler<PointInactivatedEvent>,
        INotificationHandler<PointCreatedEvent>
    {
        private readonly IPointRepository _pointRepository;

        public PointEventHandler(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        public Task Handle(PointUpdatedEvent @event, CancellationToken cancellationToken)
        {
            _pointRepository.ClearCache();

            //TODO: Dispatch Command to update all routes and connections that have a reference to update point

            return Task.CompletedTask;
        }

        public Task Handle(PointInactivatedEvent notification, CancellationToken cancellationToken)
        {
            _pointRepository.ClearCache();

            //TODO: Dispatch Command to update all routes and connections that have a reference to inactivated point

            return Task.CompletedTask;
        }

        public Task Handle(PointCreatedEvent notification, CancellationToken cancellationToken)
        {
            _pointRepository.ClearCache();

            return Task.CompletedTask;
        }
    }
}
