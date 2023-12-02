namespace ReactWithASP.Server.Models.TestDbs.SupplyChain
{
    public class Inventory
    {
        public required Guid id { get; set; } = Guid.NewGuid();
        public required string product_name { get; set; }
        public required string brand { get; set; }
        public required int quantity { get; set; }
        public required DateTime release_date {  get; set; } 
    }
}
