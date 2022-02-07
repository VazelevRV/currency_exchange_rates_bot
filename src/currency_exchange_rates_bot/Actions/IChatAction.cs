using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace currency_exchange_rates_bot.Actions {
    public interface IChatAction {
       Task ExecuteAsync(Message message, CancellationToken ct);

       bool Contains(Message message);
    }
}