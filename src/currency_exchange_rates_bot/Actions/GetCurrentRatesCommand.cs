using System.Threading;
using System.Threading.Tasks;
using currency_exchange_rates_bot.Services;
using currency_exchange_rates_bot.Models.DTO.Requests;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace currency_exchange_rates_bot.Actions
{
    public sealed class GetCurrentRatesCommand : IChatAction
    {
        private readonly CurrencyAPIService _currencyService;
        private readonly ITelegramBotClient _client;
        private const string CommandName = "Поточні";

        public GetCurrentRatesCommand(ITelegramBotClient client, CurrencyAPIService currencyService)
        {
            _client = client;
            _currencyService = currencyService;
        }

        public bool Contains(Message message)
        {
            return message.Text.StartsWith(CommandName);
        }

        public async Task ExecuteAsync(Message message, CancellationToken ct)
        {
            var response = await _currencyService.GetCurrentRatesAsync(
                new CurrentRatesRequest(){
                    Format = "format=1",
                    Symbols = "USD,UAH,RUB"
                    },
                ct);

            string messageText = $"1 {response.Base} на {response.Date.ToShortDateString()} = \n";
            foreach(var rate in response.Rates)
            {
                messageText += $"{rate.Key} {rate.Value}\n";
            }
            await _client.SendTextMessageAsync(
                      message.Chat.Id,
                      messageText,
                      cancellationToken: ct);
        }
    }
}