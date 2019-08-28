using Bogus;
using DeliveryService.Domain.CommandHandlers;
using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Domain.ValueObject;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryService.Test.Unit.Command
{
    public class RouteCommandHandlerTests
    {
        private readonly Mock<IConnectionRepository> _connectionRepository;
        private readonly Mock<IPointRepository> _pointRepository;
        private readonly Mock<IRouteRepository> _routeRepository;
        private readonly Mock<IMediator> _mediator;
        private readonly RouteCommandHandler _handler;
        private readonly Point _point;

        public RouteCommandHandlerTests()
        {
            _mediator = new Mock<IMediator>();
            _pointRepository = new Mock<IPointRepository>();
            _connectionRepository = new Mock<IConnectionRepository>();
            _routeRepository = new Mock<IRouteRepository>();
            _handler = new RouteCommandHandler(_pointRepository.Object, _routeRepository.Object, _connectionRepository.Object, _mediator.Object);
            _point = new Faker<Point>()
                .CustomInstantiator(f => new Point("A"))
                .RuleFor(p => p.Id, ObjectId.Parse("5d61646c1c86f4ef738e5e90"))
                .RuleFor(p => p.Active, true)
                .RuleFor(p => p.CreatedAt, DateTime.UtcNow)
                .RuleFor(p => p.UpdatedAt, DateTime.UtcNow)
                .Generate();
        }

        [Fact]
        public async void Handle_WithValidPoint_CreateRouteCommandSuccess()
        {
            // Arrange
            var command = new Faker<CreateRouteCommand>()
                .RuleFor(p => p.OriginPointId, v => v.Lorem.Letter())
                .RuleFor(p => p.DestinationPointId, v => v.Lorem.Letter())
                .Generate();

            _pointRepository
                .Setup(m => m.FindAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_point));

            _routeRepository
                .Setup(m => m.AlreadyExistsAsync(It.IsAny<Expression<Func<Route, bool>>>()))
                .Returns(Task.FromResult(false));

            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Value.Should().NotBe(ObjectId.Empty);
        }

    }
}
