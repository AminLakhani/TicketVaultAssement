using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCollector.Models
{
    internal class Ticker
    {
        [JsonProperty("ticker")]
        public required string Symbol { get; set; }

        [JsonProperty("no_of_comments")]
        public int NoOfComments { get; set; }

        [JsonProperty("sentiment")]
        public required string Sentiment { get; set; }

        [JsonProperty("sentiment_score")]
        public decimal? SentimentScore { get; set; }

        public DateTime Date { get; set; }
    }
}
