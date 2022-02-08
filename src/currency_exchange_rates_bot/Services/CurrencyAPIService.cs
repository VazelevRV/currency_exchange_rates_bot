using RestSharp;
using currency_exchange_rates_bot.Models.DTO.Requests;
using currency_exchange_rates_bot.Models.DTO.Responses;
using System.Threading.Tasks;
using System.Threading;

namespace currency_exchange_rates_bot.Services
{
    public sealed class CurrencyAPIService
    {
        private const string API_BASE_URI = "http://api.exchangeratesapi.io/v1/";
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
                new RestRequest($"latest?access_key={_currencyToken}&symbols={request.Symbols}&{request.Format}"),
                ct);
            return response;
        }
    }
}