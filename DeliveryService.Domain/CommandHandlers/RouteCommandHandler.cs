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
        IRequestHandler<UpdateRouteCommand, DomainResult>,
        IRequestHandler<InactiveRouteCommand, DomainResult>
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
            var origin = await _pointRepository.FindAsync(command.OriginPointId);
            var destination = await _pointRepository.FindAsync(command.DestinationPointId);

            if (origin is null || destination is null)
            {
                return DomainResult.Failure<ObjectId>("Origin or Destination not found");
            }

            var route = Route.Create(origin, destination);

            if (await _routeRepository.AlreadyExistsAsync(x => route.IsTheSame(x)))
            {
                return DomainResult.Failure<ObjectId>("Route already exists", HttpStatusCode.Conflict);
            }

            await _routeRepository.CreateAsync(route);

            return DomainResult.Ok(route.Id);
        }

        public async Task<DomainResult> Handle(UpdateRouteCommand command, CancellationToken cancellationToken)
        {
            var origin = await _pointRepository.FindAsync(command.OriginPointId);
            var destination = await _pointRepository.FindAsync(command.DestinationPointId);

            if (origin is null || destination is null)
            {
                return DomainResult.Failure<string>("Origin or Destination not found");
            }

            var route = await _routeRepository.FindAsync(command.Id);

            if (route is null)
            {
                return DomainResult.Failure<string>("Route not found");
            }

            var arePointsChanged = route.ArePointsChanged(origin, destination);

            route.Update(origin, destination);

            if(arePointsChanged && await _routeRepository.AlreadyExistsAsync(x => route.IsTheSame(x)))
            {
                return DomainResult.Failure<string>("Route already exists", HttpStatusCode.Conflict);
            }

            await _routeRepository.UpdateAsync(route);

            return DomainResult.Ok();
        }

        public async Task<DomainResult> Handle(InactiveRouteCommand command, CancellationToken cancellationToken)
        {
            var route = await _routeRepository.FindAsync(command.Id);

            if (route is null)
            {
                return DomainResult.Failure<string>("Route not found");
            }

            route.Inactive();

            await _routeRepository.UpdateAsync(route);

            return DomainResult.Ok();
        }
    }
}
