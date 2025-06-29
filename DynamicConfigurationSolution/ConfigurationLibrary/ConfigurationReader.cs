using ConfigurationLibrary.Interfaces;
using ConfigurationLibrary.Models;
using ConfigurationLibrary.Providers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DynamicConfigSolution
{
    public class ConfigurationReader
    {
        private readonly string _applicationName;
        private readonly IStorageProvider _provider;
        private readonly ConcurrentDictionary<string, ConfigurationItem> _cache;
        private readonly Timer _refreshTimer;

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalMs)
        {
            _applicationName = applicationName;
            _provider = new MongoStorageProvider(connectionString); // Sabit olarak Mongo kullanıyoruz
            _cache = new ConcurrentDictionary<string, ConfigurationItem>();

            // İlk verileri yükle
            LoadInitialData().Wait();

            // Timer ile düzenli olarak güncelle
            _refreshTimer = new Timer(refreshTimerIntervalMs);
            _refreshTimer.Elapsed += async (sender, args) => await RefreshCacheAsync();
            _refreshTimer.Start();
        }

        private async Task LoadInitialData()
        {
            try
            {
                var configs = await _provider.GetActiveConfigurationsAsync(_applicationName);
                foreach (var config in configs)
                {
                    _cache[config.Name] = config;
                }
            }
            catch
            {
                // Hata loglanabilir. İlk seferde cache boş olabilir.
            }
        }

        private async Task RefreshCacheAsync()
        {
            try
            {
                var configs = await _provider.GetActiveConfigurationsAsync(_applicationName);
                foreach (var config in configs)
                {
                    _cache[config.Name] = config;
                }
            }
            catch
            {
                // Mongo erişilemezse mevcut cache kullanılmaya devam edilir
            }
        }

        public T GetValue<T>(string key)
        {
            if (_cache.TryGetValue(key, out var config))
            {
                try
                {
                    return (T)Convert.ChangeType(config.Value, typeof(T));
                }
                catch
                {
                    throw new InvalidCastException($"Value for key '{key}' cannot be cast to type {typeof(T)}.");
                }
            }

            throw new KeyNotFoundException($"Configuration key not found: {key}");
        }
    }
}
