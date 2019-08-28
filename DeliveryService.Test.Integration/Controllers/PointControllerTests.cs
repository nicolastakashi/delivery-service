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
    public class PointControllerTests : BaseControllerTests
    {
        private readonly string _resource = "/api/points";
        private static string _id = string.Empty;

        public PointControllerTests(ServiceContainersFixture fixture)
            : base(fixture)
        {
            LoginAsAdmin().Wait();
        }

        [Fact, Order(1)]
        public async void Post_Create_Point_Success()
        {
            var command = new Faker<CreatePointCommand>()
                .RuleFor(p => p.Name, v => v.Lorem.Letter())
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
        public async void Get_Point_Success()
        {
            var result = await Client.GetAsync($"{_resource}/{_id}");

            var response = await result.ReadAsResponseAsync();

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNull();
        }

        [Fact, Order(3)]
        public async void Get_Paged_Points_Success()
        {
            var result = await Client.GetAsync(_resource);
            var response = await result.ReadAsResponseAsync<PagedQueryResult<object>>();

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Total.Should().BeGreaterThan(0);
        }


        [Fact, Order(4)]
        public async void Put_Point_Success()
        {
            var command = new Faker<UpdatePointCommand>()
                .RuleFor(p => p.Id, _id)
                .RuleFor(p => p.Name, v => v.Lorem.Letter(10))
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
