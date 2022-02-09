using currency_exchange_rates_bot.Models.DTO.Requests;

namespace currency_exchange_rates_bot.Models.DTO.Responses
{
    public class ConvertRatesResponse : ConvertRatesRequest
    {
        public ConvertRatesResponse(ConvertRatesRequest request)
        {
            From = request.From;
            To = request.To;
            Amount = request.Amount;
        }
        public double Result {get;init;}
    }
}