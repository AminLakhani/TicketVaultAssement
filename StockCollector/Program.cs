using Newtonsoft.Json;
using StockCollector.Models;
using StockCollector.Repositories;
using StockCollector.Services;
using System.Globalization;

namespace StockCollector
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var _config = new ConfigRepository();

            DateTime startDate;
            DateTime endDate;

            try
            {
                startDate = DateTime.ParseExact(_config.GetConfigValue("StartDate"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                endDate = DateTime.ParseExact(_config.GetConfigValue("EndDate"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch (FormatException x)
            {
                Console.WriteLine(x.Message);
                return;
            }

            var tickerService = new TickerService(new TickerRepository());
            var tradestieRepository = new TradestieRepository();

            DateTime date = startDate;
            while (date <= endDate)
            {
                Console.WriteLine($"Processing Data for {date.ToString("MM-dd-yyyy")}.");
                if (!tickerService.IsDataProcessed(date))
                {
                    try
                    {
                        var tickers = await tradestieRepository.GetDataByDate(date);
                        if (tickers.Count > 0)
                        {
                            tickerService.Save(tickers);
                        }
                        else
                        {
                            Console.WriteLine($"No data found for {date.ToString("MM-dd-yyyy")}.");
                        }
                    }
                    catch (Exception x)
                    {
                        Console.WriteLine(x.Message);
                    }
                }
                else
                {
                    Console.WriteLine($"Data for {date.ToString("MM-dd-yyyy")} has already been processed.");
                }

                date = date.AddDays(1);
            }
        }
    }
}
