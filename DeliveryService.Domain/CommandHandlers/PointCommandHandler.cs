using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Domain.ValueObject;
using MediatR;
using MongoDB.Bson;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Domain.CommandHandlers
{
    public class PointCommandHandler :
        IRequestHandler<CreatePointCommand, DomainResult<ObjectId>>,
        IRequestHandler<InactivePointCommand, DomainResult>,
        IRequestHandler<UpdatePointCommand, DomainResult>
    {
        private readonly IPointRepository _pointRepository;

        public PointCommandHandler(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        public async Task<DomainResult> Handle(InactivePointCommand command, CancellationToken cancellationToken)
        {
            var point = await _pointRepository.FindAsync(command.Id);

            if (point is null)
            {
                return DomainResult.Failure<string>("Point not found");
            }

            point.Inactive();

            await _pointRepository.UpdateAsync(point);

            return DomainResult.Ok();
        }

        public async Task<DomainResult<ObjectId>> Handle(CreatePointCommand command, CancellationToken cancellationToken)
        {
            var point = Point.Create(command);

            var alreadyExists = await _pointRepository.AlreadyExistsAsync(point);

            if (alreadyExists)
            {
                return DomainResult.Failure<ObjectId>("Point already exists", HttpStatusCode.Conflict);
            }

            await _pointRepository.CreateAsync(point);

            return DomainResult.Ok(point.Id);
        }

        public async Task<DomainResult> Handle(UpdatePointCommand command, CancellationToken cancellationToken)
        {
            var point = await _pointRepository.FindAsync(command.Id);

            if (point is null)
            {
                return DomainResult.Failure<string>("Point not found");
            }

            point.Update(command);

            var alreadyExists = await _pointRepository.AlreadyExistsAsync(point);

            if (alreadyExists)
            {
                return DomainResult.Failure<string>("Point already exists", HttpStatusCode.Conflict);
            }

            await _pointRepository.UpdateAsync(point);

            return DomainResult.Ok();
        }
    }
}
