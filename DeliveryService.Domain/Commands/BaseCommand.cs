using MediatR;

namespace DeliveryService.Domain.Commands
{
    public class BaseCommand<T> : IRequest<T>
    {
    }
}
