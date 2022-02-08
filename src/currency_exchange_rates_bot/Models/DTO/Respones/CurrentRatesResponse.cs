using currency_exchange_rates_bot.Models.DTO.Requests;

namespace currency_exchange_rates_bot.Models.DTO.Responses
{
    public class ConvertRatesResponse : ConvertRatesRequest
    {
        public readonly ConvertRatesRequest RequestInfo;
        public ConvertRatesResponse(ConvertRatesRequest request)
        {
            RequestInfo = request;
        }
        public double Result {get;init;}
    }
}