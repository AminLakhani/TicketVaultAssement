using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StockCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StockCollector.Repositories
{
    public interface ITickerRepository
    {
        List<Ticker> GetAll();
        Ticker GetTickersForDate(DateTime date);
        void Save(List<Ticker> tickers);
    }

    internal class TickerRepository : ITickerRepository
    {
        public List<Ticker> GetAll()
        {
            ConfigRepository config = new ConfigRepository();
            var filePath = config.GetConfigValue("TickerFilePath");
            var files = System.IO.Directory.GetFiles(filePath, "*.json");

            List<Ticker> tickers = new List<Ticker>();
            foreach (var file in files)
            {
                string json = System.IO.File.ReadAllText(file);
                var data = JsonConvert.DeserializeObject<List<Ticker>>(json);
                tickers.AddRange(data);
            }

            return tickers;
        }

        public Ticker GetTickersForDate(DateTime date)
        {
            ConfigRepository config = new ConfigRepository();
            var filePath = config.GetConfigValue("TickerFilePath");
            var month = date.ToString("MM");
            var day = date.ToString("dd");
            var year = date.ToString("yyyy");

            var file = filePath + $"{month}-{day}-{year}.json";
            if (System.IO.File.Exists(file))
            {
                string json = System.IO.File.ReadAllText(file);
                return JsonConvert.DeserializeObject<List<Ticker>>(json).FirstOrDefault();
            }

            return null;
        }

        public void Save(List<Ticker> tickers)
        {
            ConfigRepository config = new ConfigRepository();
            var filePath = config.GetConfigValue("TickerFilePath");
            var date = tickers.First().Date;
            
            string month = date.ToString("MM");
            string day = date.ToString("dd");
            var year = date.ToString("yyyy");

            string json = JsonConvert.SerializeObject(tickers);
            System.IO.File.WriteAllText(filePath + $"{month}-{day}-{year}.json", json);
        }
    }
}
