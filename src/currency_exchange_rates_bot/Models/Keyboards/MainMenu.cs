using Telegram.Bot.Types.ReplyMarkups;

namespace currency_exchange_rates_bot.Models.Keyboards
{
    public static class MainMenu
    {
        public static ReplyKeyboardMarkup GetMainMenu() => new ReplyKeyboardMarkup(new[]
        {
             new[]
             {
             new KeyboardButton("Поточні"),
             }
        });
    }
}