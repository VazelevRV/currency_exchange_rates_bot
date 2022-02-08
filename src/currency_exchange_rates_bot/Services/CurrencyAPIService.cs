using RestSharp;
using currency_exchange_rates_bot.Models.DTO.Requests;
using currency_exchange_rates_bot.Models.DTO.Responses;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace currency_exchange_rates_bot.Services
{
    public sealed class CurrencyAPIService
    {
        private const string API_BASE_URI = "https://freecurrencyapi.net/api/v2/";
        private readonly string _currencyToken;

        private readonly RestClient _apiClient;
        public CurrencyAPIService(string currencyToken)
        {
            _currencyToken = currencyToken;
            _apiClient = new RestClient(API_BASE_URI);
        }
        public async Task<CurrentRatesResponse> GetCurrentRatesAsync(CurrentRatesRequest request, CancellationToken ct)
        {
            CurrentRatesResponse response = await _apiClient.GetAsync<CurrentRatesResponse>(
                new RestRequest($"latest?apikey={_currencyToken}&base_currency={request.BaseCurrency}"),
                ct);
            return response;
        }

        public async Task<ConvertRatesResponse> ConvertRateAsync(ConvertRatesRequest request, CancellationToken ct)
        {
            CurrentRatesResponse currentRatesResponse = await _apiClient.GetAsync<CurrentRatesResponse>(
                new RestRequest($"latest?apikey={_currencyToken}&base_currency={request.From}"),
                ct);

            var exchangeRate = currentRatesResponse.Data.FirstOrDefault(r => r.Key == request.To);

            var response = new ConvertRatesResponse(request)
            {
                Result = request.Amount *  exchangeRate.Value
            };
            
            return response;
        }
    }
}