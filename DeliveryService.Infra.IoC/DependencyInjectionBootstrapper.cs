using DeliveryService.Domain.CommandHandlers;
using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Domain.Service;
using DeliveryService.Infra.Data.Context;
using DeliveryService.Infra.Data.Repositories.Write;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryService.Infra.IoC
{
    public class DependencyInjectionBootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IJwtAuthService, JwtAuthService>();

            services.AddScoped<IRequestHandler<CreateUserSessionCommand, DomainResult<string>>, AccountCommandHandler>();

            services.AddScoped<IAccountRepository, AccountRepository>();
        }
    }
}
