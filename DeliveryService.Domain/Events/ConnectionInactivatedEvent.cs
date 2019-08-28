using DeliveryService.Domain.Entities;
using MediatR;

namespace DeliveryService.Domain.Events
{
    public class ConnectionInactivatedEvent : INotification
    {
        public Connection Connection { get; set; }

        public ConnectionInactivatedEvent(Connection connection)
        {
            Connection = connection;
        }
    }
}
