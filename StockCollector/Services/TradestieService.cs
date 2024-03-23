using StockCollector.Models;
using StockCollector.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCollector.Services
{
    interface ITradestieService
    {
        Task<List<Ticker>> GetDataByDate(DateTime date);
    }

    internal class TradestieService : ITradestieService
    {
        private readonly ITradestieRepository _tradestieRepository;

        public TradestieService(ITradestieRepository tradestieRepository)
        {
            _tradestieRepository = tradestieRepository ?? new TradestieRepository();
        }

        public Task<List<Ticker>> GetDataByDate(DateTime date)
        {
            return _tradestieRepository.GetDataByDate(date);
        }
    }
}
