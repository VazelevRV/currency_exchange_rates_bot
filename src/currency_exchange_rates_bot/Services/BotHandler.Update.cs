using System.Threading;
using System.Threading.Tasks;
using currency_exchange_rates_bot.Models;
using Telegram.Bot.Types;

namespace currency_exchange_rates_bot.Services
{
    partial class BotHandler
    {
        private async Task HandleMessageAsync(BotUser u, Message m, CancellationToken ct)
        {
            await _actionPatternManager.HandleCommandAsync(u, m, ct);
        }

        private async Task HandleStateAsync(BotUser u, Message m, CancellationToken ct)
        {
            await _actionPatternManager.HandleStateAsync(u, m, ct);
        }
    }
}