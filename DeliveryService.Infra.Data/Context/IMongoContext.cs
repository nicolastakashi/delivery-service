using MongoDB.Driver;

namespace DeliveryService.Infra.Data.Context
{
    public interface IMongoContext
    {
        IMongoCollection<TDocument> GetCollection<TDocument>(string name, MongoCollectionSettings settings = null);
    }
}
