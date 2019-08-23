using DeliveryService.Infra.Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Linq;
using System.Reflection;

namespace DeliveryService.Infra.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIf(this IApplicationBuilder application, bool condition, Func<IApplicationBuilder, IApplicationBuilder> action)
           => condition ? action(application) : application;

        public static IApplicationBuilder UseCustomSwaggerUI(this IApplicationBuilder application, Type type) =>
            application.UseSwaggerUI(
                options =>
                {
                    options.DocumentTitle = type.Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
                    options.RoutePrefix = string.Empty;
                    options.DisplayRequestDuration();

                    var provider = application.ApplicationServices.GetService(typeof(IApiVersionDescriptionProvider)) as IApiVersionDescriptionProvider;
                    var apiVersions = provider.ApiVersionDescriptions.OrderByDescending(x => x.ApiVersion);

                    foreach (var apiVersion in apiVersions)
                    {
                        options.SwaggerEndpoint($"/swagger/{apiVersion.GroupName}/swagger.json", $"Version {apiVersion.ApiVersion}");
                    }
                });

        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder application)
            => application.UseMiddleware<ExceptionHandleMiddleware>();
    }
}
