using ConfigurationLibrary.Interfaces;
using ConfigurationLibrary.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLibrary.Providers
{
    public class MongoStorageProvider: IStorageProvider
    {
        private readonly IMongoCollection<ConfigurationItem> _collection;

        public MongoStorageProvider(string connectionString, string databaseName = "configDb", string collectionName = "configurations")
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<ConfigurationItem>(collectionName);
        }

        public async Task<List<ConfigurationItem>> GetActiveConfigurationsAsync(string applicationName)
        {
            var filter = Builders<ConfigurationItem>.Filter.Eq(c => c.IsActive, true) &
                         Builders<ConfigurationItem>.Filter.Eq(c => c.ApplicationName, applicationName);

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
