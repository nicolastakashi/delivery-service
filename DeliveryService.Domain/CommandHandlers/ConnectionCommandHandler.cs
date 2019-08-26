using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Domain.CommandHandlers
{
    public sealed class ConnectionCommandHandler :
        IRequestHandler<CreateConnectionCommand, DomainResult<ObjectId>>,
        IRequestHandler<UpdatedConnectionCommand, DomainResult>,
        IRequestHandler<InactiveConnectionCommand, DomainResult>
    {
        private readonly IPointRepository _pointRepository;
        private readonly IConnectionRepository _connectionRepository;

        public ConnectionCommandHandler(IPointRepository pointRepository, IConnectionRepository connectionRepository)
        {
            _pointRepository = pointRepository;
            _connectionRepository = connectionRepository;
        }

        public async Task<DomainResult<ObjectId>> Handle(CreateConnectionCommand command, CancellationToken cancellationToken)
        {
            var origin = _pointRepository.FindAsync(command.OriginPointId);
            var destination = _pointRepository.FindAsync(command.DestinationPointId);

            await Task.WhenAll(origin, destination);

            if (origin.Result is null || destination.Result is null)
            {
                return DomainResult.Failure<ObjectId>("Origin or Destination not found");
            }

            var connection = new Connection(origin.Result, destination.Result, command.Time, command.Cost);

            var alreadyExists = await _connectionRepository.AlreadyExistsAsync(connection);

            if (alreadyExists)
            {
                return DomainResult.Failure<ObjectId>("Connection already exists.");
            }

            await _connectionRepository.CreateAsync(connection);

            return DomainResult.Ok(connection.Id);
        }

        public async Task<DomainResult> Handle(UpdatedConnectionCommand command, CancellationToken cancellationToken)
        {
            var origin = _pointRepository.FindAsync(command.OriginPointId);
            var destination = _pointRepository.FindAsync(command.DestinationPointId);
            var connection = _connectionRepository.FindAsync(command.Id);

            await Task.WhenAll(origin, destination, connection);

            if (origin.Result is null || destination.Result is null)
            {
                return DomainResult.Failure<ObjectId>("Origin or Destination not found");
            }

            var connectionResult = connection.Result;
            var arePointsChanged = connectionResult.ArePointsChanged(origin.Result, destination.Result);

            connectionResult.Update(origin.Result, destination.Result, command.Time, command.Cost);

            if (arePointsChanged && await _connectionRepository.AlreadyExistsAsync(connectionResult))
            {
                return DomainResult.Failure<ObjectId>("Connection already exists.");
            }


            await _connectionRepository.UpdateAsync(connectionResult);

            return DomainResult.Ok();
        }

        public async Task<DomainResult> Handle(InactiveConnectionCommand command, CancellationToken cancellationToken)
        {
            var connection = await _connectionRepository.FindAsync(command.Id);

            connection.Inactive();

            await _connectionRepository.UpdateAsync(connection);

            return DomainResult.Ok();
        }
    }
}
