using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockCollector.Repositories;
using StockCollector.Models;
using StockCollector.Services;
using NUnit.Framework;
using Moq;


namespace StockCollector.Tests.Services
{

    [TestFixture]
    public class TickerServiceTest
    {
        private Mock<ITickerRepository> _mockRepository;
        private TickerService _tickerService;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<ITickerRepository>();
            _tickerService = new TickerService(_mockRepository.Object);
        }

        [Test]
        public void GetAll_ReturnsAllTickers()
        {
            // Arrange
            var expectedTickers = new List<Ticker>();
            expectedTickers.Add(new Ticker() { Symbol = "AAPL", NoOfComments = 10, Sentiment = "Positive", SentimentScore = 0.5m, Date = DateTime.Now });
            expectedTickers.Add(new Ticker() { Symbol = "GOOGL", NoOfComments = 20, Sentiment = "Negative", SentimentScore = -0.5m, Date = DateTime.Now });

            _mockRepository.Setup(repo => repo.GetAll()).Returns(expectedTickers);

            // Act
            var result = _tickerService.GetAll();

            // Assert
            Assert.AreEqual(expectedTickers, result);
        }


        [Test]
        public void GetTickersForDate_ReturnsTickersForSpecifiedDate()
        {
            // Arrange
            var date = new DateTime(2024, 3, 23);
            var expectedTicker = new Ticker { Symbol = "AAPL", Date = date };
            _mockRepository.Setup(repo => repo.GetTickersForDate(date)).Returns(expectedTicker);

            // Act
            var result = _tickerService.GetTickersForDate(date);

            // Assert
            Assert.AreEqual(expectedTicker, result);
        }

        [Test]
        public void IsDataProcessed_ReturnsTrueIfDataExistsForDate()
        {
            // Arrange
            var date = new DateTime(2024, 3, 23);
            _mockRepository.Setup(repo => repo.GetTickersForDate(date)).Returns(new Ticker() { Symbol = "AAPL"});

            // Act
            var result = _tickerService.IsDataProcessed(date);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsDataProcessed_ReturnsFalseIfNoDataExistsForDate()
        {
            // Arrange
            var date = new DateTime(2024, 3, 23);
            _mockRepository.Setup(repo => repo.GetTickersForDate(date)).Returns((Ticker)null);

            // Act
            var result = _tickerService.IsDataProcessed(date);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Save_SavesTickers()
        {
            // Arrange
            var tickers = new List<Ticker> { new Ticker { Symbol = "AAPL" } };
            _mockRepository.Setup(repo => repo.Save(tickers));

            // Act
            _tickerService.Save(tickers);

            // Assert
            _mockRepository.Verify(repo => repo.Save(tickers), Times.Once);
        }
    }
}
