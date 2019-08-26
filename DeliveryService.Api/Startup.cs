using DeliveryService.Infra.Api.Extensions;
using DeliveryService.Infra.Data.Seeding;
using DeliveryService.Infra.IoC;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJwtAuthentication(Configuration)
                .AddCustomSwagger()
                .AddCustomApiVersioning()
                .AddVersionedApiExplorer(x => x.GroupNameFormat = "'v'VVV")
                .AddCustomResponseCompression()
                .AddMediatR(typeof(Startup))
                .AddCustomMvcCore()
                .AddHealthChecks();

            DependencyInjectionBootstrapper.RegisterServices(services);
            SeeddingContext.Seed(Configuration, HostingEnvironment);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseHealthChecks("/health")
                .UseExceptionHandlerMiddleware()
                .UseResponseCompression()
                .UseForwardedHeaders()
                .UseIf(env.IsProduction(), x => x.UseHsts())
                .UseAuthentication()
                .UseMvc()
                .UseSwagger()
                .UseCustomSwaggerUI(typeof(Startup));
        }
    }
}
