using Telegram.Bot.Types.ReplyMarkups;

namespace currency_exchange_rates_bot.Models.Keyboards
{
    public static class KeyboardsManager
    {
        public static ReplyKeyboardMarkup GetMainMenu() => new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("Поточні курси")
            },
            new[]
            {
                new KeyboardButton("Конвертувати")
            }
        });

        public static ReplyKeyboardMarkup GetCodesSelectionMenu() => new ReplyKeyboardMarkup(new []
        {
            new[]
            {
                new KeyboardButton("UAH")
            },
            new[]
            {
                new KeyboardButton("USD")
            },
            new[]
            {
                new KeyboardButton("EUR")
            },
            new[]
            {
                new KeyboardButton("RUB")
            },
            new[]
            {
                new KeyboardButton("CAD")
            }
        });
    }
}