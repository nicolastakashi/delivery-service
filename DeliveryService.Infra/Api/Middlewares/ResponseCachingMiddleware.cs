using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Api.Middlewares
{
    public class ResponseCachingMiddleware
    {
        private readonly RequestDelegate next;

        public ResponseCachingMiddleware(RequestDelegate next)
            => this.next = next;

        public async Task Invoke(HttpContext context)
        {
            context.Response.GetTypedHeaders()
                .CacheControl = new CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(10)
                };

            context.Response.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };

            var responseCachingFeature = context.Features.Get<IResponseCachingFeature>();

            if (responseCachingFeature != null)
            {
                var queryKeys = context.Request.Query.Keys;
                var keys = new string[queryKeys.Count];

                queryKeys.CopyTo(keys, 0);

                responseCachingFeature.VaryByQueryKeys = keys;
            }

            await next(context);
        }

    }
}
