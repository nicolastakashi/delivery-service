using Xunit;

namespace DeliveryService.Test.Integration.Infra
{
    [CollectionDefinition("DeliveryServiceTests")]
    public class DeliveryServiceTestCollectionFixture : ICollectionFixture<ServiceContainersFixture>
    {
    }
}
