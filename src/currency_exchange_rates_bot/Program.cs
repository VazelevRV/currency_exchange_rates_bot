using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using currency_exchange_rates_bot.Services;
using currency_exchange_rates_bot.Actions;
using Microsoft.EntityFrameworkCore;
using System;
using currency_exchange_rates_bot.Models;
using System.Reflection;

namespace currency_exchange_rates_bot
{
    internal static class Program
    {
        private static async Task Main()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment.EnvironmentName;
                    builder
                        .AddJsonFile("botsettings.json", false, false)
                        .AddJsonFile($"botsettings.{env}.json", true, false)
                        .AddJsonFile("currency_api_settings.json", false, false)
                        .AddJsonFile($"currency_api_settings.{env}.json", true, false);
                })
                .ConfigureServices((context, services) =>
                {
                    var config = context.Configuration;
                    services.AddLogging();

                    services.AddDbContext<CurrencyExchangeDbContext>(options =>
                    options.UseSqlite("Filename=Application.db", builder =>
                    {
                        builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                    }));

                    services.AddSingleton<ActionPatternManager>();
                    services.AddSingleton<ITelegramBotClient>(
                        new TelegramBotClient(config["BOT_TOKEN"]));
                    services.AddSingleton<IUpdateHandler, BotHandler>();
                    services.AddHostedService<BotHandlerService>();
                    services.AddSingleton<CurrencyAPIService>(new CurrencyAPIService(config["CURRENCY_TOKEN"]));

                    

                    var baseType = typeof(IChatAction);
                    foreach (var commandType in baseType.Assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t) && t.IsClass && t.IsPublic && !t.IsAbstract))
                    {
                        services.AddScoped(baseType, commandType);
                    }
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConfiguration(context.Configuration);
                    builder.AddConsole();
                    builder.AddDebug();
                })
                .Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                CurrencyExchangeDbContext context = scope.ServiceProvider.GetRequiredService<CurrencyExchangeDbContext>();

                //if(File.Exists("Application.db"))
                //    File.Delete("Application.db");

                // Застосувати міграції
                if (context.Database.GetMigrations().Any())
                    context.Database.Migrate();

                // Впевнитись, що база створена
                context.Database.EnsureCreated();
            }

            await host.RunAsync();
        }
    }
}
