using Bogus;
using DeliveryService.Domain.CommandHandlers;
using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Domain.ValueObject;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryService.Test.Unit.Command
{
    public class PointCommandHandlerTests
    {
        private readonly Mock<IPointRepository> _pointRepositoryMock;
        private readonly Point _point;
        private readonly PointCommandHandler _handler;

        public PointCommandHandlerTests()
        {
            _point = new Faker<Point>()
                .CustomInstantiator(f => new Point("A"))
                .RuleFor(p => p.Id, ObjectId.Parse("5d61646c1c86f4ef738e5e90"))
                .RuleFor(p => p.Active, true)
                .RuleFor(p => p.CreatedAt, DateTime.UtcNow)
                .RuleFor(p => p.UpdatedAt, DateTime.UtcNow)
                .Generate();

            _pointRepositoryMock = new Mock<IPointRepository>();
            _handler = new PointCommandHandler(_pointRepositoryMock.Object);
        }

        [Fact]
        public async void Handle_WithValidPoint_CreatePointCommandSuccess()
        {
            // Arrange
            _pointRepositoryMock
                .Setup(m => m.PointAlreadyExistsAsync(It.IsAny<Point>()))
                .Returns(Task.FromResult(false));

            var command = new Faker<CreatePointCommand>()
                .RuleFor(p => p.Name, v => v.Lorem.Letter())
                .Generate();

            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Value.Should().NotBe(ObjectId.Empty);
        }

        [Fact]
        public async void Handle_WithExistPoint_CreatePointCommandSuccess()
        {
            // Arrange
            _pointRepositoryMock
                .Setup(m => m.PointAlreadyExistsAsync(It.IsAny<Point>()))
                .Returns(Task.FromResult(true));

            var command = new Faker<CreatePointCommand>()
                .RuleFor(p => p.Name, "A")
                .Generate();

            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Value.Should().Be(ObjectId.Empty);
        }

        [Fact]
        public async void Handle_WithValiPoint_InactivePointCommandSuccess()
        {
            // Arrange
            _pointRepositoryMock
                .Setup(m => m.FindAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_point));

            var command = new Faker<InactivePointCommand>()
                .RuleFor(p => p.Id, v => v.Lorem.Random.AlphaNumeric(10))
                .Generate();

            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Fact]
        public async void Handle_WithInValiPoint_InactivePointCommandSuccess()
        {
            // Arrange
            _pointRepositoryMock
                .Setup(m => m.FindAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<Point>(null));


            var command = new Faker<InactivePointCommand>()
                .RuleFor(p => p.Id, v => v.Lorem.Random.AlphaNumeric(10))
                .Generate();

            // Act
            var result = await _handler.Handle(command, new CancellationToken());


            // Assert
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async void Handle_WithValiPoint_UpdatePointCommandSuccess()
        {
            // Arrange
            _pointRepositoryMock
                .Setup(m => m.FindAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_point));

            _pointRepositoryMock
                .Setup(m => m.PointAlreadyExistsAsync(It.IsAny<Point>()))
                .Returns(Task.FromResult(false));

            var command = new Faker<UpdatePointCommand>()
                .RuleFor(p => p.Id, v => v.Lorem.Random.AlphaNumeric(10))
                .RuleFor(p => p.Name, v => v.Lorem.Letter())
                .Generate();

            // Act
            var result = await _handler.Handle(command, new CancellationToken());


            // Assert
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Fact]
        public async void Handle_WithInValiPoint_UpdatePointCommandError()
        {
            // Arrange
            _pointRepositoryMock
                .Setup(m => m.FindAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<Point>(null));

            var command = new Faker<UpdatePointCommand>()
                .RuleFor(p => p.Id, v => v.Lorem.Random.AlphaNumeric(10))
                .RuleFor(p => p.Name, v => v.Lorem.Letter())
                .Generate();

            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async void Handle_WithExistedValiPoint_UpdatePointCommandSuccess()
        {
            // Arrange
            _pointRepositoryMock
                .Setup(m => m.FindAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(It.IsAny<Point>()));

            _pointRepositoryMock
              .Setup(m => m.PointAlreadyExistsAsync(It.IsAny<Point>()))
              .Returns(Task.FromResult(true));

            var command = new Faker<UpdatePointCommand>()
                .RuleFor(p => p.Id, v => v.Lorem.Random.AlphaNumeric(10))
                .RuleFor(p => p.Name, v => v.Lorem.Letter())
                .Generate();

            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNullOrEmpty();
        }
    }
}
