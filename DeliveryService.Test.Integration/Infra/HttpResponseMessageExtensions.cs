using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeliveryService.Test.Integration.Infra
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<Response<TResult>> ReadAsResponseAsync<TResult>(this HttpResponseMessage httpResponseMessage)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response<TResult>>(content);
        }

        public static async Task<Response<object>> ReadAsResponseAsync(this HttpResponseMessage httpResponseMessage)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response<object>>(content);
        }
    }
}
