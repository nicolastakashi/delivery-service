using Bogus;
using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Queries.Result;
using DeliveryService.Test.Integration.Infra;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Extensions.Ordering;

namespace DeliveryService.Test.Integration.Controllers
{
    [Collection("DeliveryServiceTests")]
    public class ConnectionControllerTests : BaseControllerTests
    {
        private readonly string _resource = "/api/connections";
        private static string _id = string.Empty;

        public ConnectionControllerTests(ServiceContainersFixture fixture) 
            : base(fixture)
        {
            LoginAsAdmin().Wait();
        }

        [Fact, Order(1)]
        public async void Post_Connections_Success()
        {
            var command = new Faker<CreateConnectionCommand>()
                .RuleFor(p => p.OriginPointId, "5d650c106692258d95b7b79b")
                .RuleFor(p => p.DestinationPointId, "5d650c2e51806b9679d33d51")
                .RuleFor(p => p.Time, v => v.Random.Number(70))
                .RuleFor(p => p.Cost, v => v.Random.Number(50))
                .Generate();

            using (var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"))
            {
                var result = await Client.PostAsync(_resource, content);
                var response = await result.ReadAsResponseAsync<string>();

                result.StatusCode.Should().Be(HttpStatusCode.Created);
                response.Result.Should().NotBeNullOrWhiteSpace();

                _id = response.Result;
            }
        }

        [Fact, Order(2)]
        public async void Get_Connection_Success()
        {
            var result = await Client.GetAsync($"{_resource}/{_id}");

            var response = await result.ReadAsResponseAsync();

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNull();
        }

        [Fact, Order(3)]
        public async void Get_Paged_Connections_Success()
        {
            var result = await Client.GetAsync(_resource);
            var response = await result.ReadAsResponseAsync<PagedQueryResult<object>>();

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Total.Should().BeGreaterThan(0);
        }

        [Fact, Order(4)]
        public async void Put_Point_Success()
        {
            var command = new Faker<UpdatedConnectionCommand>()
                .RuleFor(p => p.Id, _id)
                .RuleFor(p => p.OriginPointId, "5d650c2e51806b9679d33d51")
                .RuleFor(p => p.DestinationPointId, "5d650c211ea6e3f067031715")
                .RuleFor(p => p.Time, v => v.Random.Number(70))
                .RuleFor(p => p.Cost, v => v.Random.Number(50))
                .Generate();

            using (var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"))
            {
                var result = await Client.PutAsync(_resource, content);

                result.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Fact, Order(5)]
        public async void Delete_Point_Success()
        {
            var result = await Client.DeleteAsync($"{_resource}/{_id}");
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
