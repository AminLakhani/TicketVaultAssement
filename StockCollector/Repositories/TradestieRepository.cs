using Newtonsoft.Json;
using StockCollector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCollector.Repositories
{
    interface ITradestieRepository
    {
        Task<List<Ticker>> GetDataByDate(DateTime date);
    }

    internal class TradestieRepository : ITradestieRepository
    {
        public const string apiURL = @"https://tradestie.com/api/v1/apps/reddit?date=";

        public async Task<List<Ticker>> GetDataByDate(DateTime date)
        {
            ConfigRepository config = new ConfigRepository();
            string apiURL = config.GetConfigValue("API-URL");
            string url = apiURL + date.ToString("yyyy-MM-dd");

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var top50TickersForDay = JsonConvert.DeserializeObject<List<Ticker>>(content);
                    if (top50TickersForDay?.Count > 0)
                    {
                        top50TickersForDay.Select(s => { s.Date = date; return s; }).ToList();
                        return top50TickersForDay;
                    }
                    else
                    {
                        return new List<Ticker>();
                    }
                }
                else
                {
                    return new List<Ticker>();
                }
            }  
        }
    }
}
