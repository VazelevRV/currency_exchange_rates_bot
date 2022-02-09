using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using currency_exchange_rates_bot.Actions;
using Telegram.Bot.Types;
using currency_exchange_rates_bot.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace currency_exchange_rates_bot.Services
{
    public sealed class ActionPatternManager
    {
        private readonly IServiceProvider _provider;
        private readonly ILoggerFactory _loggerFactory;

        public ActionPatternManager(IServiceProvider provider, ILoggerFactory loggerFactory)
        {
            _provider = provider;
            _loggerFactory = loggerFactory;
        }

        public async Task HandleCommandAsync(BotUser user, Message m, CancellationToken ct)
        {
            using var scope = _provider.CreateScope();
            var scopedProvider = scope.ServiceProvider;

            IEnumerable<IChatAction> commands = scopedProvider.GetServices<IChatAction>()
                                                .Where(a => a.ActionType == ActionTypes.command);
            foreach (var command in commands)
            {
                if (command.Contains(user, m))
                {
                    try
                    {
                        await command.ExecuteAsync(user, m, ct);
                    }
                    catch (Exception e)
                    {
                        var logger = _loggerFactory.CreateLogger(command.GetType());
                        logger.LogError(e, "Exception occurred while running the command");
                    }
                    break;
                }
            }
        }

        public async Task HandleStateAsync(BotUser user, Message m, CancellationToken ct)
        {
            using var scope = _provider.CreateScope();
            var scopedProvider = scope.ServiceProvider;

            IEnumerable<IChatAction> stateHandlers = scopedProvider.GetServices<IChatAction>()
                                                .Where(a => a.ActionType == ActionTypes.state);
            foreach (var stateHandler in stateHandlers)
            {
                if (stateHandler.Contains(user, m))
                {
                    try
                    {
                        await stateHandler.ExecuteAsync(user, m, ct);
                    }
                    catch (Exception e)
                    {
                        var logger = _loggerFactory.CreateLogger(stateHandler.GetType());
                        logger.LogError(e, "Exception occurred while running the state handler");
                    }
                    break;
                }
            }
        }
    }
}