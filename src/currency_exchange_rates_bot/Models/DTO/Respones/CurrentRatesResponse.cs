using System;
using System.Collections.Generic;

namespace currency_exchange_rates_bot.Models.DTO.Responses
{
    public partial class CurrentRatesResponse
    {
        public bool Success { get; set; }
        public long Timestamp { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, double> Rates { get; set; }
    }
}