using ConfigurationLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLibrary.Interfaces
{
    public interface IStorageProvider
    {
        Task<List<ConfigurationItem>> GetActiveConfigurationsAsync(string applicationName);
    }
}
