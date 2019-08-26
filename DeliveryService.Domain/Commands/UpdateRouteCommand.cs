using DeliveryService.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Commands
{
    public class UpdateRouteCommand : BaseCommand<DomainResult>
    {
        [Required(ErrorMessage = "Origin is required")]
        public string OriginPointId { get; set; }

        [Required(ErrorMessage = "Destination is required")]
        public string DestinationPointId { get; set; }
    }
}
