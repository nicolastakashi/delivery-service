using DeliveryService.Infra.Api.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Api.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandleMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = JsonConvert.SerializeObject(EnvelopeResponse.Error(ex.Message));
                await httpContext.Response.WriteAsync(response);
            }
        }
    }
}
