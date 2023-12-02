namespace ReactWithASP.Server.Models.TestDbs.SupplyChain
{
    public class Products
    {
        public required Guid id { get; set; } = Guid.NewGuid();
        public required string product_name { get; set; }
        public required string brand { get; set; }
        public required string model { get; set; }
        public required double price { get; set; }
        public required string color { get; set; }
        public required double weight { get; set; }
        public required string dimensions { get; set; }
        public required DateTime release_date { get; set; }
        public required string description { get; set; }
        public required double rating { get; set; }
    }
}
