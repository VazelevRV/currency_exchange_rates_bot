using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace currency_exchange_rates_bot.Actions
{
    public sealed class HelloWorldCommand : IChatAction
    {
        private ITelegramBotClient Client { get; }
        private const string CommandName = "/helloworld";

        public HelloWorldCommand(ITelegramBotClient client)
        {
            Client = client;
        }

        public bool Contains(Message message)
        {
            return message.Text.StartsWith(CommandName);
        }

        public async Task ExecuteAsync(Message message, CancellationToken ct)
        {
            await Client.SendTextMessageAsync(
                      message.Chat.Id,
                      "Hello world!",
                      cancellationToken: ct);
        }
    }
}