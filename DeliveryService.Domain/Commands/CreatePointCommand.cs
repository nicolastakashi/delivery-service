using DeliveryService.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Commands
{
    public class CreatePointCommand : BaseCommand<DomainResult>
    {
        [Required(ErrorMessage = "Name")]
        public string Name { get; set; }
    }
}
