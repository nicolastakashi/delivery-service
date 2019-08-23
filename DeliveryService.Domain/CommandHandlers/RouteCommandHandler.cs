using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Domain.CommandHandlers
{
    public class RouteCommandHandler :
        IRequestHandler<CreateRouteCommand, DomainResult>
    {
        private readonly IPointRepository _pointRepository;

        public RouteCommandHandler(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        public async Task<DomainResult> Handle(CreateRouteCommand command, CancellationToken cancellationToken)
        {
            var originTask = _pointRepository.FindAsync(command.OriginPointId);
            var destinationTask = _pointRepository.FindAsync(command.DestinationPointId);

            await Task.WhenAll(originTask, destinationTask);

            var origin = originTask.Result;
            var destination = destinationTask.Result;

            if (origin is null || destination is null)
            {
                return DomainResult.Failure<string>("Origin or Destination not found");
            }

            var route = Route.Create(command, origin, destination);




            return DomainResult.Ok();
        }
    }
}
