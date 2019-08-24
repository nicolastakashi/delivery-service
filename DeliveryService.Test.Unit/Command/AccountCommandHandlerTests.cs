using Bogus;
using DeliveryService.Domain.CommandHandlers;
using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Domain.Service;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryService.Test.Unit.Command
{
    public class AccountCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IJwtAuthService> _jwtAuthServiceMock;
        private readonly AccountCommandHandler _handler;

        public AccountCommandHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _jwtAuthServiceMock = new Mock<IJwtAuthService>();
            _handler = new AccountCommandHandler(_accountRepositoryMock.Object, _jwtAuthServiceMock.Object);
            _accountRepositoryMock
                .Setup(x => x.GetUserByEmailAndPassword("nicolas@hotmail.com", "123"))
                .Returns(Task.FromResult(new User("Nicolas", "nicolas@hotmail.com", "123", UserRole.Admin)));
        }

        [Fact]
        public async void Handle_WithValidUser_CreatesUserSessionSuccess()
        {
            //Arrange
            var command = new Faker<CreateUserSessionCommand>()
                .RuleFor(p => p.Email, "nicolas@hotmail.com")
                .RuleFor(p => p.Password, "123")
                .Generate();

            //Act
            var result = await _handler.Handle(command, new CancellationToken());

            //Assert
            result.Success
                .Should()
                .BeTrue();
        }

        [Fact]
        public async void Handle_WithInvalidUser_CreatesUserSessionError()
        {
            //Arrange
            var command = new Faker<CreateUserSessionCommand>()
                .RuleFor(p => p.Email, v => v.Person.Email)
                .RuleFor(p => p.Password, v => v.Internet.Password())
                .Generate();

            //Act
            var result = await _handler.Handle(command, new CancellationToken());

            //Assert
            result.Success
                .Should()
                .BeFalse();
        }
    }
}
