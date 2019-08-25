using DeliveryService.Domain.Entities;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Commands
{
    public sealed class CreateConnectionCommand : BaseCommand<DomainResult<ObjectId>>
    {
        [Required(ErrorMessage = "Origin is required")]
        public string OriginPointId { get; set; }

        [Required(ErrorMessage = "Destination is required")]
        public string DestinationPointId { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Time { get; set; }

        [Range(1,double.MaxValue)]
        public decimal Cost { get; set; }
    }
}
