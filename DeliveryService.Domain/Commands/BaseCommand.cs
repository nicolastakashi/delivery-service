using DeliveryService.Domain.Entities;
using DeliveryService.Infra.Attributes;
using MediatR;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace DeliveryService.Domain.Commands
{
    public class BaseCommand<T> : IRequest<T>
    {
        [SwaggerExclude]
        [JsonIgnore]
        public ObjectId UserId { get; set; }
    }
}
