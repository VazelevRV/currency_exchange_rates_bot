using System.Threading;
using System.Threading.Tasks;
using currency_exchange_rates_bot.Models;
using Telegram.Bot.Types;

namespace currency_exchange_rates_bot.Actions {
    public interface IChatAction {
       Task ExecuteAsync(BotUser user, Message message, CancellationToken ct);

       bool Contains(Message message);
    }
}