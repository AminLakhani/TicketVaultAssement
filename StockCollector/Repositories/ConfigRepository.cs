using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCollector.Repositories
{
    public interface IConfigRepository
    {
        string GetConfigValue(string key);
    }

    public class ConfigRepository : IConfigRepository
    {
        static IConfiguration Config = new ConfigurationBuilder()
               .AddJsonFile("appSettings.json")
               .Build();

        public string GetConfigValue(string key)
        {
            return Config.GetSection(key)?.Value;
        }
    }
}
