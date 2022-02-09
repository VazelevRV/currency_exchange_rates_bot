using System.Threading;
using System.Threading.Tasks;
using currency_exchange_rates_bot.Models;
using Telegram.Bot.Types;

namespace currency_exchange_rates_bot.Actions {
    public interface IChatAction {
       ActionTypes ActionType {get;init;}
       Task ExecuteAsync(BotUser user, Message message, CancellationToken ct);
       bool Contains(BotUser user, Message message);
    }

    public enum ActionTypes
    {
        command,
        state,
    }
}