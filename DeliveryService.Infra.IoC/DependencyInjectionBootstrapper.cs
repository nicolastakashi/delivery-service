using DeliveryService.Domain.CommandHandlers;
using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.EventHandlers;
using DeliveryService.Domain.Events;
using DeliveryService.Domain.Repositories.Readonly;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Domain.Service;
using DeliveryService.Domain.ValueObject;
using DeliveryService.Infra.Data.Context;
using DeliveryService.Infra.Data.Repositories.ReadOnly;
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
            services.AddScoped<IRedisContext, RedisContext>();
            services.AddScoped<IJwtAuthService, JwtAuthService>();

            RegisterCommands(services);
            RegisterRepositories(services);
            RegisterEvents(services);
        }

        private static void RegisterEvents(IServiceCollection services)
        {
            services.AddScoped<INotificationHandler<PointCreatedEvent>, PointEventHandler>();
            services.AddScoped<INotificationHandler<PointUpdatedEvent>, PointEventHandler>();
            services.AddScoped<INotificationHandler<PointInactivatedEvent>, PointEventHandler>();

            services.AddScoped<INotificationHandler<ConnectionCreatedEvent>, ConnectionEventHandler>();
            services.AddScoped<INotificationHandler<ConnectionUpdatedEvent>, ConnectionEventHandler>();
            services.AddScoped<INotificationHandler<ConnectionInactivatedEvent>, ConnectionEventHandler>();

            services.AddScoped<INotificationHandler<RouteCreatedEvent>, RouteEventHandler>();
            services.AddScoped<INotificationHandler<RouteUpdatedEvent>, RouteEventHandler>();
            services.AddScoped<INotificationHandler<RouteInactivatedEvent>, RouteEventHandler>();
        }

        private static void RegisterCommands(IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<CreateUserSessionCommand, DomainResult<string>>, AccountCommandHandler>();

            services.AddScoped<IRequestHandler<CreatePointCommand, DomainResult<ObjectId>>, PointCommandHandler>();
            services.AddScoped<IRequestHandler<InactivePointCommand, DomainResult>, PointCommandHandler>();
            services.AddScoped<IRequestHandler<UpdatePointCommand, DomainResult>, PointCommandHandler>();

            services.AddScoped<IRequestHandler<CreateConnectionCommand, DomainResult<ObjectId>>, ConnectionCommandHandler>();
            services.AddScoped<IRequestHandler<UpdatedConnectionCommand, DomainResult>, ConnectionCommandHandler>();
            services.AddScoped<IRequestHandler<InactiveConnectionCommand, DomainResult>, ConnectionCommandHandler>();

            services.AddScoped<IRequestHandler<CreateRouteCommand, DomainResult<ObjectId>>, RouteCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateRouteCommand, DomainResult>, RouteCommandHandler>();
            services.AddScoped<IRequestHandler<InactiveRouteCommand, DomainResult>, RouteCommandHandler>();
            services.AddScoped<IRequestHandler<FindTheBestRoutePathCommand, DomainResult<BestRoutePath>>, RouteCommandHandler>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IPointRepository, PointRepository>();
            services.AddScoped<IPointReadOnlyRepository, PointReadOnlyRepository>();
            services.AddScoped<IConnectionRepository, ConnectionRepository>();
            services.AddScoped<IConnectionReadOnlyRepository, ConnectionReadOnlyRepository>();
            services.AddScoped<IRouteReadOnlyRepository, RouteReadOnlyRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
        }
    }
}
