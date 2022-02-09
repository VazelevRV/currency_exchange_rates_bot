using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using currency_exchange_rates_bot.Models;
using currency_exchange_rates_bot.Models.Keyboards;

namespace currency_exchange_rates_bot.Actions.Commands
{
    public sealed class StartConvertCommand : IChatAction
    {
        public ActionTypes ActionType {get;init;} = ActionTypes.command;
        private readonly CurrencyExchangeDbContext _context;
        private readonly ITelegramBotClient _client;
        private const string CommandName = "Конвертувати";

        public StartConvertCommand(ITelegramBotClient client, CurrencyExchangeDbContext context)
        {
            _client = client;
            _context = context;
        }

        public bool Contains(BotUser user, Message message)
        {
            return message.Text.StartsWith(CommandName);
        }

        public async Task ExecuteAsync(BotUser user, Message message, CancellationToken ct)
        {
            user.State = "PendingFromCode";
            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync(ct);

            await _client.SendTextMessageAsync(
                      message.Chat.Id,
                      "Оберіть валюту що необхідно ковертувати",
                      replyMarkup: KeyboardsManager.GetCodesSelectionMenu(),
                      cancellationToken: ct);
        }
    }
}