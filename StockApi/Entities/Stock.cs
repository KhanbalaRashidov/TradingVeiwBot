namespace StockApi.Entities
{
    public class Stock
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? CompanyName { get; set; }
        public string? Price { get; set; }
        public string? PercentageOfChange { get; set; }
        public string? ChangePrice { get; set; }
        
    }
}
