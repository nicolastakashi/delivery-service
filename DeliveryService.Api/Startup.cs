using DeliveryService.Infra.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJwtAuthentication(Configuration);
            services.AddCustomSwagger();
            services.AddCustomApiVersioning();
            services.AddVersionedApiExplorer(x => x.GroupNameFormat = "'v'VVV");
            services.AddCustomResponseCompression();
            services.AddCustomMvcCore();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
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
