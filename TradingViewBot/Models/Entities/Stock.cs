using System.ComponentModel.DataAnnotations;

namespace TradingViewBot.Models.Entities
{
    public class Stock
    {
        [Key] 
        public Guid Id { get; set; }
        
        public  string StockCode { get; set; }
   
        public double Price { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

    }
}
