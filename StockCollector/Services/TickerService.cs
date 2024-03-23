using StockCollector.Models;
using StockCollector.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCollector.Services
{
    public interface ITickerService
    {
        List<Ticker> GetAll();
        Ticker GetTickersForDate(DateTime date);
        void Save(List<Ticker> tickers);
    }

    public class TickerService : ITickerService
    {
        private readonly ITickerRepository _tickerRepository;

        public TickerService(ITickerRepository tickerRepository)
        {
            _tickerRepository = tickerRepository ?? new TickerRepository();
        }

        public List<Ticker> GetAll()
        {
            return _tickerRepository.GetAll();
        }

        public Ticker GetTickersForDate(DateTime date)
        {
            return _tickerRepository.GetTickersForDate(date);
        }

        public bool IsDataProcessed(DateTime date)
        {
            return _tickerRepository.GetTickersForDate(date) != null;
        }

        public void Save(List<Ticker> tickers)
        {
            _tickerRepository.Save(tickers);
        }
    }
}
