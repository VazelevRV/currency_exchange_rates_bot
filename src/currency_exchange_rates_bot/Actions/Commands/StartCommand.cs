using System.Threading;
using System.Threading.Tasks;
using currency_exchange_rates_bot.Models;
using currency_exchange_rates_bot.Models.Keyboards;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace currency_exchange_rates_bot.Actions.Commands
{
    public sealed class StartCommand : IChatAction
    {
        public ActionTypes ActionType {get;init;} = ActionTypes.command;
        private readonly ITelegramBotClient _client;
        private const string CommandName = "/start";

        public StartCommand(ITelegramBotClient client)
        {
            _client = client;
        }

        public bool Contains(BotUser user, Message message)
        {
            return message.Text.StartsWith(CommandName);
        }

        public async Task ExecuteAsync(BotUser user, Message message, CancellationToken ct)
        {
            await _client.SendTextMessageAsync(
                      message.Chat.Id,
                      "Оберіть функцію",
                      replyMarkup: KeyboardsManager.GetMainMenu(),
                      cancellationToken: ct);
        }
    }
}