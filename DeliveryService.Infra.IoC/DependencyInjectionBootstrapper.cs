using DeliveryService.Domain.CommandHandlers;
using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Domain.Service;
using DeliveryService.Infra.Data.Context;
using DeliveryService.Infra.Data.Repositories.Write;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

namespace DeliveryService.Infra.IoC
{
    public class DependencyInjectionBootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IJwtAuthService, JwtAuthService>();

            services.AddScoped<IRequestHandler<CreateUserSessionCommand, DomainResult<string>>, AccountCommandHandler>();

            services.AddScoped<IRequestHandler<CreatePointCommand, DomainResult<ObjectId>>, PointCommandHandler>();
            services.AddScoped<IRequestHandler<InactivePointCommand, DomainResult>, PointCommandHandler>();
            services.AddScoped<IRequestHandler<UpdatePointCommand, DomainResult>, PointCommandHandler>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IPointRepository, PointRepository>();
        }
    }
}
