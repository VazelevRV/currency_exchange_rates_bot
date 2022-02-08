using System;

namespace currency_exchange_rates_bot.Models

{
    public sealed record BotUser
    {
        public int Id {get;init;}
        public long TelegramId {get;init;}
        public string State{get;set;}
    }
}