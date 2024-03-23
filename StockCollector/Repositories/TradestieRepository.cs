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
                    return ParseData(await response.Content.ReadAsStringAsync(), date);
                }
                else
                {
                    int retryCount = 0;
                    while (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests && retryCount < 3)
                    {
                        Console.WriteLine("Too many requests. Waiting for 5 seconds.");
                        await Task.Delay(5000);
                        response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            return ParseData(await response.Content.ReadAsStringAsync(), date);
                        }
                        retryCount++;
                    }
                }

                return new List<Ticker>();
            }
        }

        private static List<Ticker> ParseData(string content, DateTime date)
        {
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
    }
}
