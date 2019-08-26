using Bogus;
using DeliveryService.Domain.Commands;
using DeliveryService.Test.Integration.Infra;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace DeliveryService.Test.Integration.Controllers
{
    [Collection("DeliveryServiceTests")]
    public class AccountControllerTests
    {
        private readonly HttpClient _client;
        private readonly string _resource = "/api/accounts";

        public AccountControllerTests(ServiceContainersFixture fixture)
        {
            _client = fixture.GetClient();
        }

        [Fact]
        public async void Post_Accounts_WithValidUser_Success()
        {
            // Arrange
            var command = new CreateUserSessionCommand { Email = "admin@deliveryservice.com", Password = "admin" };

            //Act
            using (var body = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"))
            using (var result = await _client.PostAsync(_resource, body))
            {
                var response = await result.ReadAsResponseAsync<string>();

                //Assert
                response.IsSuccess.Should().BeTrue();
                response.Result.Should().NotBeNullOrWhiteSpace();
            }
        }

        [Fact]
        public async void Post_Accounts_WithInValidUser_Success()
        {
            // Arrange
            var command = new Faker<CreateUserSessionCommand>()
                .RuleFor(p => p.Email, v => v.Person.Email)
                .RuleFor(p => p.Password, v => v.Internet.Password())
                .Generate();

            //Act
            using (var body = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"))
            using (var result = await _client.PostAsync(_resource, body))
            {
                var response = await result.ReadAsResponseAsync<string>();

                //Assert
                result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
                response.IsSuccess.Should().BeFalse();
                response.Result.Should().BeNullOrWhiteSpace();
            }
        }
    }
}
