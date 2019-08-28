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
    public class ConnectionCommandHandlerTests
    {
        private readonly Mock<IPointRepository> _pointRepositoryMock;
        private readonly Mock<IConnectionRepository> _connectionRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConnectionCommandHandler _handler;
        private readonly Point _origin;
        private readonly Point _destination;

        public ConnectionCommandHandlerTests()
        {
            _pointRepositoryMock = new Mock<IPointRepository>();
            _connectionRepositoryMock = new Mock<IConnectionRepository>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new ConnectionCommandHandler(_pointRepositoryMock.Object, _connectionRepositoryMock.Object, _mediatorMock.Object);

            _origin = new Faker<Point>().CustomInstantiator(f => new Point("A")).Generate();
            _destination = new Faker<Point>().CustomInstantiator(f => new Point("C")).Generate();
        }

        [Fact]
        public async void Handle_WithValidConnection_CreateConnectionCommandSuccess()
        {
            // Arrange
            var command = BuildCreateConnectionCommand();

            SetupPointRepositoryFindMethod(command.OriginPointId, command.DestinationPointId);
            SetupAlreadyExistsConnection(false);


            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Value.Should().NotBe(ObjectId.Empty);
        }

        [Fact]
        public async void Handle_WithUnexistedPoint_CreateConnectionCommandSuccess()
        {
            // Arrange
            _pointRepositoryMock
                .Setup(m => m.FindAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<Point>(null));

            var command = BuildCreateConnectionCommand();

            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Value.Should().Be(ObjectId.Empty);
        }

        [Fact]
        public async void Handle_WithExistedConnection_CreateConnectionCommandSuccess()
        {
            // Arrange
            var command = BuildCreateConnectionCommand();

            SetupPointRepositoryFindMethod(command.OriginPointId, command.DestinationPointId);
            SetupAlreadyExistsConnection(true);

            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Value.Should().Be(ObjectId.Empty);
        }

        [Fact]
        public async void Handle_WithValidConnection_UpdatedConnectionCommandSuccess()
        {
            // Arrange
            var connection = new Faker<Connection>().CustomInstantiator(c => new Connection(_origin, _destination, c.Random.Number(), c.Random.Number())).Generate();
            var command = BuildUpdatedConnectionCommand();

            SetupPointRepositoryFindMethod(command.OriginPointId, command.DestinationPointId);
            SetupAlreadyExistsConnection(false);

            _connectionRepositoryMock
                .Setup(m => m.FindAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(connection));

            var result = await _handler.Handle(command, new CancellationToken());

            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public async void HandleWithUnexistedPoint_UpdatedConnectionCommandSuccess()
        {
            // Range
            var connection = new Faker<Connection>().CustomInstantiator(c => new Connection(_origin, _destination, c.Random.Number(), c.Random.Number())).Generate();
            var command = BuildUpdatedConnectionCommand();

            _pointRepositoryMock.Setup(m => m.FindAsync(It.IsAny<string>())).Returns(Task.FromResult<Point>(null));

            var result = await _handler.Handle(command, new CancellationToken());

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        }

        private void SetupPointRepositoryFindMethod(string originId, string destinationId)
        {
            _pointRepositoryMock
               .Setup(m => m.FindAsync(originId))
               .Returns(Task.FromResult(_origin));

            _pointRepositoryMock
                .Setup(m => m.FindAsync(destinationId))
                .Returns(Task.FromResult(_destination));
        }

        private void SetupAlreadyExistsConnection(bool alreadyExists)
        {
            _connectionRepositoryMock
                .Setup(m => m.AlreadyExistsAsync(It.IsAny<Expression<Func<Connection, bool>>>()))
                .Returns(Task.FromResult(alreadyExists));
        }

        private CreateConnectionCommand BuildCreateConnectionCommand()
            => new Faker<CreateConnectionCommand>()
                .RuleFor(p => p.OriginPointId, v => v.Lorem.Random.AlphaNumeric(10))
                .RuleFor(p => p.DestinationPointId, v => v.Lorem.Random.AlphaNumeric(10))
                .RuleFor(p => p.Time, v => v.Random.Number())
                .RuleFor(p => p.Cost, v => v.Random.Number())
                .Generate();

        private UpdatedConnectionCommand BuildUpdatedConnectionCommand()
            => new Faker<UpdatedConnectionCommand>()
                .RuleFor(p => p.Id, v => v.Lorem.Random.AlphaNumeric(10))
                .RuleFor(p => p.OriginPointId, v => v.Lorem.Random.AlphaNumeric(10))
                .RuleFor(p => p.DestinationPointId, v => v.Lorem.Random.AlphaNumeric(10))
                .RuleFor(p => p.Time, v => v.Random.Number())
                .RuleFor(p => p.Cost, v => v.Random.Number())
                .Generate();
    }
}
