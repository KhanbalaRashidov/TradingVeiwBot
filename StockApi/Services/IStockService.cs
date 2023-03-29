using StockApi.Entities;

namespace StockApi.Services
{
    public interface IStockService
    {
        Task<IEnumerable<Stock>> GetAll();
    }
}
