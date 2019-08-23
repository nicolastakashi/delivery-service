using DeliveryService.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Commands
{
    public class CreateRouteCommand : BaseCommand<DomainResult>
    {
        [Required(ErrorMessage = "Origin is required")]
        public string OriginPointId { get; set; }

        [Required(ErrorMessage = "Destination is required")]
        public string DestinationPointId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Time must be greater than zero")]
        public int Time { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Cost must be greater than zero")]
        public int Cost { get; set; }
    }
}
