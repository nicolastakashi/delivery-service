using DeliveryService.Domain.Entities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Commands
{
    public class UpdatePointCommand : BaseCommand<DomainResult>
    {
        [JsonIgnore]
        public string Id { get; set; }

        [Required(ErrorMessage = "Name")]
        public string Name { get; set; }
    }
}
