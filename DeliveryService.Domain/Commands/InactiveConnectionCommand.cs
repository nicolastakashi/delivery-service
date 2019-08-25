using DeliveryService.Domain.Entities;

namespace DeliveryService.Domain.Commands
{
    public sealed class InactiveConnectionCommand : BaseCommand<DomainResult>
    {
        public string Id { get; set; }

        public InactiveConnectionCommand(string id)
        {
            Id = id;
        }

        public static InactiveConnectionCommand Create(string id)
            => new InactiveConnectionCommand(id);
    }
}
