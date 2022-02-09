using System;
using System.Threading;
using System.Threading.Tasks;
using currency_exchange_rates_bot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace currency_exchange_rates_bot.Services
{
    public sealed partial class BotHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _client;
        private readonly ActionPatternManager _actionPatternManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BotHandler> _logger;

        public BotHandler(ITelegramBotClient client, ActionPatternManager actionPatternManager, IServiceProvider serviceProvider, ILogger<BotHandler> logger)
        {
            _client = client;
            _actionPatternManager = actionPatternManager;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedProvider = scope.ServiceProvider;

            BotUser user;

            try
            {
                var context = scopedProvider.GetRequiredService<CurrencyExchangeDbContext>();
                user = await EnsureUserExistsAsync(update.Message, context, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred while ensuring that user exists");
                return;
            }

            if (user.State != "Default")
            {
                await (update switch
                {
                    { Message: { Chat: { Type: ChatType.Private }, Text: { } } } =>
                        HandleStateAsync(user, update.Message, cancellationToken),
                    _ => Task.CompletedTask
                });
                return;
            }

            await (update switch
            {
                { Message: { Chat: { Type: ChatType.Private }, Text: { } } } =>
                    HandleMessageAsync(user, update.Message, cancellationToken),
                _ => Task.CompletedTask
            });
        }

        public Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception);
            return Task.CompletedTask;
        }

        public UpdateType[] AllowedUpdates => new[] { UpdateType.Message };

        private static async Task<BotUser> EnsureUserExistsAsync(Message m, CurrencyExchangeDbContext context, CancellationToken ct)
        {
            var user = await context.BotUsers
                .FirstOrDefaultAsync(u => u.TelegramId == m.From.Id, ct);

            if (user == null)
            {
                var newUser = new BotUser
                {
                    TelegramId = m.From.Id,
                    State = "Default",
                    StateData = ""
                };
                context.BotUsers.Add(newUser);
                await context.SaveChangesAsync(ct);
            }

            return user;
        }
    }
}