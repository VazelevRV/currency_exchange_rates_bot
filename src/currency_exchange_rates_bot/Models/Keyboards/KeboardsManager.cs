using Telegram.Bot.Types.ReplyMarkups;

namespace currency_exchange_rates_bot.Models.Keyboards
{
    public static class KeyboardsManager
    {
        public static ReplyKeyboardMarkup GetMainMenu() => new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("Поточні курси"),
                new KeyboardButton("Конвертувати")
            }
        });

        public static ReplyKeyboardMarkup GetCodesSelectionMenu() => new ReplyKeyboardMarkup(new []
        {
            new[]
            {
                new KeyboardButton("USD"),
                new KeyboardButton("JPY"), 
                new KeyboardButton("CNY"), 
                new KeyboardButton("CHF"), 
                new KeyboardButton("CAD"), 
                new KeyboardButton("MXN"), 
                new KeyboardButton("INR")
            },
            new[]
            { 
                new KeyboardButton("BRL"), 
                new KeyboardButton("RUB"), 
                new KeyboardButton("KRW"), 
                new KeyboardButton("IDR"), 
                new KeyboardButton("TRY"),
                new KeyboardButton("SAR"),
                new KeyboardButton("SEK")
            }
        });
    }
}