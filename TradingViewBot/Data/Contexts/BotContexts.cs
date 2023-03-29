using Microsoft.EntityFrameworkCore;
using TradingViewBot.Models;
using TradingViewBot.Models.Entities;

namespace TradingViewBot.Data.Contexts
{
    public class BotContexts : DbContext
    {
        public BotContexts(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
    }
}
