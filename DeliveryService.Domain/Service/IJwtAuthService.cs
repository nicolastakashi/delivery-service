using DeliveryService.Domain.Entities;

namespace DeliveryService.Domain.Service
{
    public interface IJwtAuthService
    {
        string CreateJwtToken(User user);
    }
}
