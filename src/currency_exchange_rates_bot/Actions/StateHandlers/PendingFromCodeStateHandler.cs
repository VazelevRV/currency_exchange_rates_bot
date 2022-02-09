using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using currency_exchange_rates_bot.Models;
using currency_exchange_rates_bot.Models.Keyboards;

namespace currency_exchange_rates_bot.Actions.StateHandlers
{
    public sealed class PendingFromCodeStateHandler : IChatAction
    {
        public ActionTypes ActionType {get;init;} = ActionTypes.state;
        private readonly CurrencyExchangeDbContext _context;
        private readonly ITelegramBotClient _client;
        private const string StateCode = "PendingFromCode";

        public PendingFromCodeStateHandler(ITelegramBotClient client, CurrencyExchangeDbContext context)
        {
            _client = client;
            _context = context;
        }

        public bool Contains(BotUser user, Message message)
        {
            return user.State == StateCode;
        }

        public async Task ExecuteAsync(BotUser user, Message message, CancellationToken ct)
        {
            var pendingFromCode = message.Text.Trim();
            if(pendingFromCode.Length != 3)
            {
                await _client.SendTextMessageAsync(
                      message.Chat.Id,
                      "Невірний код валюти, оберіть ще раз",
                      replyMarkup: KeyboardsManager.GetCodesSelectionMenu(),
                      cancellationToken: ct);
                      return;
            }

            user.State = "PendingToCode";
            user.StateData = pendingFromCode;
            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync(ct);

            await _client.SendTextMessageAsync(
                      message.Chat.Id,
                      "Оберіть валюту, в яку необхідно конвертувати",
                      replyMarkup: KeyboardsManager.GetCodesSelectionMenu(),
                      cancellationToken: ct);
        }
    }
}