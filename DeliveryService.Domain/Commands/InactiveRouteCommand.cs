using DeliveryService.Domain.Entities;

namespace DeliveryService.Domain.Commands
{
    public class InactiveRouteCommand : BaseCommand<DomainResult>
    {
        public InactiveRouteCommand(string id)
        {
            Id = id;
        }

        protected InactiveRouteCommand()
        {
        }

        public string Id { get; set; }

        public static InactiveRouteCommand Create(string id)
            => new InactiveRouteCommand(id);
    }
}
