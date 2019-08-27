using DeliveryService.Domain.Commands;
using DeliveryService.Test.Integration.Infra;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Test.Integration.Controllers
{
    public class BaseControllerTests
    {
        protected readonly HttpClient Client;

        public BaseControllerTests(Infra.ServiceContainersFixture fixture)
        {
            Client = fixture.GetClient();
        }

        public async Task LoginAsAdmin()
        {
            var command = new CreateUserSessionCommand { Email = "admin@deliveryservice.com", Password = "admin" };

            using (var body = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"))
            using (var result = await Client.PostAsync("/api/accounts/", body))
            {
                result.EnsureSuccessStatusCode();

                var response = await result.ReadAsResponseAsync<string>();

                if (Client.DefaultRequestHeaders.Contains("Authorization")) Client.DefaultRequestHeaders.Remove("Authorization");

                Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {response.Result}");
            }
        }

    }
}
