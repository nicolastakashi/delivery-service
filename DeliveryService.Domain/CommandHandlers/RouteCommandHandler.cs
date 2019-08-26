using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using MediatR;
using MongoDB.Bson;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Domain.CommandHandlers
{
    public class RouteCommandHandler :
        IRequestHandler<CreateRouteCommand, DomainResult<ObjectId>>,
        IRequestHandler<UpdateRouteCommand, DomainResult>
    {
        private readonly IPointRepository _pointRepository;
        private readonly IRouteRepository _routeRepository;

        public RouteCommandHandler(IPointRepository pointRepository, IRouteRepository routeRepository)
        {
            _pointRepository = pointRepository;
            _routeRepository = routeRepository;
        }

        public async Task<DomainResult<ObjectId>> Handle(CreateRouteCommand command, CancellationToken cancellationToken)
        {
            var originTask = _pointRepository.FindAsync(command.OriginPointId);
            var destinationTask = _pointRepository.FindAsync(command.DestinationPointId);

            await Task.WhenAll(originTask, destinationTask);

            var origin = originTask.Result;
            var destination = destinationTask.Result;

            if (origin is null || destination is null)
            {
                return DomainResult.Failure<ObjectId>("Origin or Destination not found");
            }

            var route = Route.Create(origin, destination);

            if(await _routeRepository.AlreadyExistsAsync(route))
            {
                return DomainResult.Failure<ObjectId>("Route already exists", HttpStatusCode.Conflict);
            }

            await _routeRepository.CreateAsync(route);

            return DomainResult.Ok(route.Id);
        }

        public Task<DomainResult> Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
