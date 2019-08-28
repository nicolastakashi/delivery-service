using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Events;
using DeliveryService.Domain.Repositories.Write;
using MediatR;
using MongoDB.Bson;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Domain.CommandHandlers
{
    public sealed class ConnectionCommandHandler :
        IRequestHandler<CreateConnectionCommand, DomainResult<ObjectId>>,
        IRequestHandler<UpdatedConnectionCommand, DomainResult>,
        IRequestHandler<InactiveConnectionCommand, DomainResult>
    {
        private readonly IMediator _mediator;
        private readonly IPointRepository _pointRepository;
        private readonly IConnectionRepository _connectionRepository;

        public ConnectionCommandHandler(IPointRepository pointRepository, IConnectionRepository connectionRepository, IMediator mediator)
        {
            _pointRepository = pointRepository;
            _connectionRepository = connectionRepository;
            _mediator = mediator;
        }

        public async Task<DomainResult<ObjectId>> Handle(CreateConnectionCommand command, CancellationToken cancellationToken)
        {
            var origin = await _pointRepository.FindAsync(command.OriginPointId);
            var destination = await _pointRepository.FindAsync(command.DestinationPointId);

            if (origin is null || destination is null)
            {
                return DomainResult.Failure<ObjectId>("Origin or Destination not found");
            }

            var connection = new Connection(origin, destination, command.Time, command.Cost);

            var alreadyExists = await _connectionRepository.AlreadyExistsAsync(x => connection.IsTheSame(x));

            if (alreadyExists)
            {
                return DomainResult.Failure<ObjectId>("Connection already exists.", HttpStatusCode.Conflict);
            }

            await _connectionRepository.CreateAsync(connection);

            await _mediator.Publish(new ConnectionCreatedEvent(connection));

            return DomainResult.Ok(connection.Id);
        }

        public async Task<DomainResult> Handle(UpdatedConnectionCommand command, CancellationToken cancellationToken)
        {
            var origin = await _pointRepository.FindAsync(command.OriginPointId);
            var destination = await _pointRepository.FindAsync(command.DestinationPointId);

            if (origin is null || destination is null)
            {
                return DomainResult.Failure<string>("Origin or Destination not found");
            }

            if (origin.Equals(destination))
            {
                return DomainResult.Failure<string>("You can't create a connection to the same origin and destination");
            }

            var connection = await _connectionRepository.FindAsync(command.Id);


            if (connection is null)
            {
                return DomainResult.Failure<string>("Connection not found");
            }

            var arePointsChanged = connection.ArePointsChanged(origin, destination);

            connection.Update(origin, destination, command.Time, command.Cost);

            if (arePointsChanged && await _connectionRepository.AlreadyExistsAsync(x => connection.IsTheSame(x)))
            {
                return DomainResult.Failure<string>("Connection already exists.", HttpStatusCode.Conflict);
            }

            await _connectionRepository.UpdateAsync(connection);

            await _mediator.Publish(new ConnectionUpdatedEvent(connection));

            return DomainResult.Ok();
        }

        public async Task<DomainResult> Handle(InactiveConnectionCommand command, CancellationToken cancellationToken)
        {
            var connection = await _connectionRepository.FindAsync(command.Id);

            if (connection is null)
            {
                return DomainResult.Failure<string>("Connection not found");
            }

            connection.Inactive();

            await _connectionRepository.UpdateAsync(connection);
            await _mediator.Publish(new ConnectionInactivatedEvent(connection));

            return DomainResult.Ok();
        }
    }
}
