using DeliveryService.Domain.Entities;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Commands
{
    public class CreatePointCommand : BaseCommand<DomainResult<ObjectId>>
    {
        [Required(ErrorMessage = "Name")]
        public string Name { get; set; }
    }
}
