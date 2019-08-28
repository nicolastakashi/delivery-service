using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace DeliveryService.Infra.Data.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly MongoClient _client;

        public IMongoDatabase Db
           => _client.GetDatabase("deliveryservice");

        public IMongoCollection<TDocument> GetCollection<TDocument>(string name, MongoCollectionSettings settings = null)
            => Db.GetCollection<TDocument>(name, settings);

        public MongoContext(IConfiguration configuration)
        {
            RegisterConventions();

            _client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        }

        private void RegisterConventions()
        {
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true),
                new CamelCaseElementNameConvention(),
            };

            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }
    }
}
