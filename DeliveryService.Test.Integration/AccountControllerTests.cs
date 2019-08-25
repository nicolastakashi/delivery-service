using DeliveryService.Api;
using DeliveryService.Domain.Commands;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Xunit;

namespace DeliveryService.Test.Integration
{
    public class AccountControllerTests
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public AccountControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async void Test1()
        {
            var command = new CreateUserSessionCommand { Email = "admin@deliveryservice.com", Password = "admin" };

            using (var body = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"))
            using (var result = await _client.PostAsync("/api/accounts", body))
            {
                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Response<string>>(content);

                response.IsSuccess.Should().BeTrue();
                response.Result.Should().NotBeNullOrWhiteSpace();
            }
        }
    }
}
