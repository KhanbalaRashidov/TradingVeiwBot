using Microsoft.EntityFrameworkCore;
using StockApi.Entities;

namespace StockApi.Data
{
    public class StocksContext : DbContext
    {
        public StocksContext(DbContextOptions options) : base(options)
        {

        }

        public  DbSet<Stock> Stocks { get; set; }
    }
}
