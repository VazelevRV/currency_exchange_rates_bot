using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using currency_exchange_rates_bot.Models;
using currency_exchange_rates_bot.Models.Keyboards;
using currency_exchange_rates_bot.Services;
using currency_exchange_rates_bot.Models.DTO.Requests;
using System;

namespace currency_exchange_rates_bot.Actions.StateHandlers
{
    public sealed class PendingAmountStateHandler : IChatAction
    {
        public ActionTypes ActionType { get; init; } = ActionTypes.state;
        private readonly CurrencyAPIService _currencyService;
        private readonly CurrencyExchangeDbContext _context;
        private readonly ITelegramBotClient _client;
        private const string StateCode = "PendingAmount";

        public PendingAmountStateHandler(ITelegramBotClient client, CurrencyExchangeDbContext context, CurrencyAPIService currencyService)
        {
            _client = client;
            _context = context;
            _currencyService = currencyService;
        }

        public bool Contains(BotUser user, Message message)
        {
            return user.State == StateCode;
        }

        public async Task ExecuteAsync(BotUser user, Message message, CancellationToken ct)
        {

            var splitedData = user.StateData.Split();
            double amount;
            bool conversionSucceed = double.TryParse(message.Text.Trim(), out amount);


            if (splitedData.Length != 2)
            {
                await _client.SendTextMessageAsync(
                      message.Chat.Id,
                      "Щось пішло не так, будь ласка почніть спочатку.",
                      replyMarkup: KeyboardsManager.GetMainMenu(),
                      cancellationToken: ct);

                user.State = "Default";
                user.StateData = "";
                _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync(ct);

                return;
            }

            if (conversionSucceed == false)
            {
                await _client.SendTextMessageAsync(
                      message.Chat.Id,
                      "Невірний формат, введіть будь ласка, ще раз",
                      replyMarkup: KeyboardsManager.GetCodesSelectionMenu(),
                      cancellationToken: ct);
                return;
            }

            ConvertRatesRequest crr = new ConvertRatesRequest()
            {
                From = splitedData[0],
                To = splitedData[1],
                Amount = amount
            };

            try
            {
                var converted = await _currencyService.ConvertRateAsync(crr, ct);

                await _client.SendTextMessageAsync(
                          message.Chat.Id,
                          $"{converted.Amount}{converted.From} = {Math.Round(converted.Result, 2)}{converted.To}",
                          replyMarkup: KeyboardsManager.GetMainMenu(),
                          cancellationToken: ct);

                user.State = "Default";
                user.StateData = "";
                _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync(ct);
            }
            catch(Exception)
            {
                await _client.SendTextMessageAsync(
                          message.Chat.Id,
                          "Щось пішло не так, будь ласка почніть спочатку.",
                          replyMarkup: KeyboardsManager.GetMainMenu(),
                          cancellationToken: ct);

                user.State = "Default";
                user.StateData = "";
                _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync(ct);             
            }
        }
    }
}