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

namespace currency_exchange_rates_bot.Services
{
    public sealed class CommandPatternManager
    {
        private readonly IServiceProvider _provider;
        private readonly ILoggerFactory _loggerFactory;

        public CommandPatternManager(IServiceProvider provider, ILoggerFactory loggerFactory)
        {
            _provider = provider;
            _loggerFactory = loggerFactory;
        }

        public async Task HandleCommandAsync(Message m, CancellationToken ct)
        {
            using var scope = _provider.CreateScope();
            var scopedProvider = scope.ServiceProvider;
            BotUser user;

            try
            {
                var context = scopedProvider.GetRequiredService<CurrencyExchangeDbContext>();
                user = await EnsureUserExistsAsync(m, context, ct);
            }
            catch (Exception e)
            {
                var logger = _loggerFactory.CreateLogger<CommandPatternManager>();
                logger.LogError(e, "Exception occurred while ensuring that user or chat is up-to-date");
                return;
            }

            IEnumerable<IChatAction> commands = scopedProvider.GetServices<IChatAction>();
            foreach (var command in commands)
            {
                if (command.Contains(m))
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

        private static async Task<BotUser> EnsureUserExistsAsync(Message m, CurrencyExchangeDbContext context, CancellationToken ct)
        {
            var user = await context.BotUsers
                .FirstOrDefaultAsync(u => u.TelegramId == m.From.Id, ct);

            if (user == null)
            {
                var newUser = new BotUser
                {
                    TelegramId = m.From.Id,
                    State = "Default"
                };
                // ReSharper disable once MethodHasAsyncOverloadWithCancellation
                context.BotUsers.Add(newUser);
                await context.SaveChangesAsync(ct);
            }

            return user;
        }
    }
}