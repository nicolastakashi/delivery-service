using DeliveryService.Domain.Entities;
using MediatR;

namespace DeliveryService.Domain.Events
{
    public class ConnectionUpdatedEvent : INotification
    {
        public Connection Connection { get; set; }

        public ConnectionUpdatedEvent(Connection connection)
        {
            Connection = connection;
        }
    }
}
