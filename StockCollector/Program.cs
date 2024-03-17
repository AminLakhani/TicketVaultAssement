using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockCollector.Models;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StockCollector
{
    class Program
    {
        public const string apiURL = @"https://tradestie.com/api/v1/apps/reddit?date=";
        public const string filePath = @"C:\wwwroot\TicketVaultAssement\StockCollector\Data\";

        static async Task Main(string[] args)
        {
            //PARAMS - Need to be passed in from the command line and not hard coded.
            DateTime startingDate = new DateTime(2023, 4, 1);
            DateTime endingDate = new DateTime(2023, 4, 5);

            DateTime date = startingDate;

            try 
            {
                while (date <= endingDate)
                {
                    if (!HasDateBeenProcessed(date))
                    {
                        Console.WriteLine($"Processing Data for {date.ToString("MM-dd")}.");
                        string url = apiURL + date.ToString("yyyy-MM-dd");

                        // Perform the API request
                        var top50TickersForDay = await GetData(url);
                        if (top50TickersForDay != null)
                        {
                            top50TickersForDay.Select(s => { s.Date = date; return s; }).ToList();
                            SaveToFile(top50TickersForDay, date);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Data for {date.ToString("MM-dd")} has already been processed.");
                    }
                    date = date.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void SaveToFile(List<Ticker> data, DateTime date)
        {
            string month = date.ToString("MM");
            string day = date.ToString("dd");

            string json = JsonConvert.SerializeObject(data);
            System.IO.File.WriteAllText(filePath + $"{month}-{day}.json", json);
        }

        static bool HasDateBeenProcessed(DateTime date)
        {
            string month = date.ToString("MM");
            string day = date.ToString("dd");

            return System.IO.File.Exists(filePath + $"{month}-{day}.json");
        }

        static async Task<List<Ticker>> GetData(string apiUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(apiUrl);

                if (response != null)
                {
                    string json = response;
                    var data = JsonConvert.DeserializeObject<List<Ticker>>(json);
                    return data;
                }

                return null;
            }
        }
    }
}
