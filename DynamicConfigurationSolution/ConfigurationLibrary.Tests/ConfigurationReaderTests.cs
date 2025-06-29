using ConfigurationLibrary.Models;
using DynamicConfigSolution;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLibrary.Tests
{
    public class ConfigurationReaderTests
    {
        [Fact]
        public void GetValue_ReturnsCorrectStringValue()
        {
            // Arrange
            var reader = CreateTestReaderWithCache("SiteName", "string", "soty.io");

            // Act
            var value = reader.GetValue<string>("SiteName");

            // Assert
            Assert.Equal("soty.io", value);
        }

        [Fact]
        public void GetValue_ThrowsException_WhenKeyNotFound()
        {
            var reader = CreateTestReaderWithCache();

            Assert.Throws<KeyNotFoundException>(() => reader.GetValue<string>("MissingKey"));
        }

        private ConfigurationReader CreateTestReaderWithCache(string key = null, string type = null, string value = null)
        {
            var reader = (ConfigurationReader)Activator.CreateInstance(
                typeof(ConfigurationReader),
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new object[] { "APP", "mongodb://localhost:27017", 60000 },
                null);

            var field = typeof(ConfigurationReader).GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance);
            var cache = new ConcurrentDictionary<string, ConfigurationItem>();

            if (key != null)
            {
                cache[key] = new ConfigurationItem
                {
                    Name = key,
                    Type = type,
                    Value = value,
                    IsActive = true,
                    ApplicationName = "APP"
                };
            }

            field.SetValue(reader, cache);

            return reader;
        }
    }
}
