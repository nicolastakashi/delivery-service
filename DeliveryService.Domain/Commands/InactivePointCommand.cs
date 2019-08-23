using DeliveryService.Domain.Entities;

namespace DeliveryService.Domain.Commands
{
    public class InactivePointCommand : BaseCommand<DomainResult>
    {
        protected InactivePointCommand(string id) 
            => Id = id;

        public string Id { get; set; }

        public static InactivePointCommand Create(string id)
            => new InactivePointCommand(id);
    }
}
