using DeliveryService.Domain.Entities;
using MediatR;

namespace DeliveryService.Domain.Events
{
    public class ConnectionCreatedEvent : INotification
    {
        public Connection Connection { get; set; }

        public ConnectionCreatedEvent(Connection connection)
        {
            Connection = connection;
        }
    }
}
