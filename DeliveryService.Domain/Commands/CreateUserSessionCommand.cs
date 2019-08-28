using DeliveryService.Domain.Entities;
using DeliveryService.Infra.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Commands
{
    public class CreateUserSessionCommand : BaseCommand<DomainResult<string>>
    {
        [EmailAddress(ErrorMessage = "E-mail is not valid")]
        [Required(ErrorMessage = "E-mail is required")]
        public string Email { get; set; }

        [Password]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
