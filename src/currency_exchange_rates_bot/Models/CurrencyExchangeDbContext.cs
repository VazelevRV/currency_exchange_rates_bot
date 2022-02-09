using Microsoft.EntityFrameworkCore;

namespace currency_exchange_rates_bot.Models
{
    // Цей клас вказує Entity Framework Що потрібно створити таблиці при створенні бази
    public class CurrencyExchangeDbContext : DbContext
    {
        public CurrencyExchangeDbContext(DbContextOptions<CurrencyExchangeDbContext> options)
            : base(options)
        {
        }

        // Таблиця представляєтся як список користувачів у Entiti Framework підході
        public DbSet<BotUser> BotUsers { get; set; }
    }
}