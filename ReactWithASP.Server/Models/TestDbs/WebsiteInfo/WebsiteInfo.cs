namespace ReactWithASP.Server.Models.TestDbs.WebsiteInfo
{
    public class WebsiteInfo
    {
        public required Guid id { get; set; } = Guid.NewGuid();
        public required string website_name {  get; set; }
        public required string website_url { get; set; }
        public required string website_description {  get; set; }
        public required string website_category { get; set; }
        public required string website_logo { get; set; }
    }
}
