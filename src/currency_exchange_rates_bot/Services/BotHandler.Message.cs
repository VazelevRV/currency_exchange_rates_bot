using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace currency_exchange_rates_bot.Services
{
    partial class BotHandler
    {
        private async Task HandleMessageAsync(Message m, CancellationToken ct)
        {
            await _commandPatternManager.HandleCommandAsync(m, ct);
        }
    }
}