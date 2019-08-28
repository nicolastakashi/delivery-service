using DeliveryService.Domain.Entities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Commands
{
    public class UpdatePointCommand : BaseCommand<DomainResult>
    {
        [Required(ErrorMessage = "Id is required")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
