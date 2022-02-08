using System.Threading;
using System.Threading.Tasks;
using currency_exchange_rates_bot.Models.Keyboards;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace currency_exchange_rates_bot.Actions
{
    public sealed class StartCommand : IChatAction
    {
        private readonly ITelegramBotClient _client;
        private const string CommandName = "/start";

        public StartCommand(ITelegramBotClient client)
        {
            _client = client;
        }

        public bool Contains(Message message)
        {
            return message.Text.StartsWith(CommandName);
        }

        public async Task ExecuteAsync(Message message, CancellationToken ct)
        {
            await _client.SendTextMessageAsync(
                      message.Chat.Id,
                      "Оберіть функцію",
                      replyMarkup: MainMenu.GetMainMenu(),
                      cancellationToken: ct);
        }
    }
}