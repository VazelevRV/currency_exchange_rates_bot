using System;
using System.Collections.Generic;

namespace currency_exchange_rates_bot.Models.DTO.Responses
{
    public sealed class CurrentRatesResponse
    {
        public Dictionary<string, double> Data { get; set; }
    }
}