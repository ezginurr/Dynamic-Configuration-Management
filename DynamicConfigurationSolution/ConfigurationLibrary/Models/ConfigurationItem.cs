using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLibrary.Models
{
    public class ConfigurationItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public bool IsActive { get; set; }

        public string ApplicationName { get; set; }
    }
}
