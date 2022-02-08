
namespace currency_exchange_rates_bot.Models.DTO.Requests
{
    public class ConvertRatesRequest
    {
        public string From {get;init;}
        public string To {get;init;}
        public double Amount {get;init;}
    }
}