
namespace currency_exchange_rates_bot.Models.DTO.Requests
{
    public record CurrentRatesRequest
    {
        public string BaseCurrency {get;init;}
    }
}