using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using StockApi.Data;
using StockApi.Entities;

namespace StockApi.Services
{
    public class StockService : IStockService
    {
        private readonly IServiceProvider _serviceProvider;

        public StockService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<Stock>> GetAll()
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<StocksContext>();

            return await context.Set<Stock>().AsNoTracking().ToListAsync();
        }
    }
}
