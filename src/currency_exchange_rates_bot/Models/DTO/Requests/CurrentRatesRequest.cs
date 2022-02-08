using System;

namespace currency_exchange_rates_bot.Models.DTO.Requests
{
    public record CurrentRatesRequest
    {
        public string Symbols {get;init;}
        public string Format {get;init;}
        public string Base {get;init;}
    }
}