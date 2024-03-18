using Newtonsoft.Json;
using StockCollector.Models;

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
            var httpClient = new HttpClient();

            try
            {
                while (date <= endingDate)
                {
                    if (!HasDateBeenProcessed(date))
                    {
                        Console.WriteLine($"Processing Data for {date.ToString("MM-dd")}.");
                        string url = apiURL + date.ToString("yyyy-MM-dd");

                        var top50TickersForDay = await GetData(url, httpClient);
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
                httpClient.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                httpClient.Dispose();
            }
        }

        private static void SaveToFile(List<Ticker> data, DateTime date)
        {
            string month = date.ToString("MM");
            string day = date.ToString("dd");

            string json = JsonConvert.SerializeObject(data);
            System.IO.File.WriteAllText(filePath + $"{month}-{day}.json", json);
        }

        private static bool HasDateBeenProcessed(DateTime date)
        {
            string month = date.ToString("MM");
            string day = date.ToString("dd");

            return System.IO.File.Exists(filePath + $"{month}-{day}.json");
        }

        private static async Task<List<Ticker>> GetData(string apiUrl, HttpClient httpClient)
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
