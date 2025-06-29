using ConfigurationLibrary.Models;
using MongoDB.Driver;

namespace ConfigurationWebUI.Services
{
    public class MongoConfigService
    {
        private readonly IMongoCollection<ConfigurationItem> _collection;

        public MongoConfigService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb");
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("configDb");
            _collection = db.GetCollection<ConfigurationItem>("configurations");
        }

        public async Task<List<ConfigurationItem>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<ConfigurationItem> GetByIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(ConfigurationItem item)
        {
            await _collection.InsertOneAsync(item);
        }

        public async Task UpdateAsync(ConfigurationItem item)
        {
            await _collection.ReplaceOneAsync(x => x.Id == item.Id, item);
        }
    }
}
