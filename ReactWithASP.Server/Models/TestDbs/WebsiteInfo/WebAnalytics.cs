namespace ReactWithASP.Server.Models.TestDbs.WebsiteInfo
{
    public class WebAnalytics
    {
        public required Guid id { get; set; } = Guid.NewGuid();
        public required int user_id {  get; set; }
        public required string page_url { get; set; }
        public required int visit_duration { get; set; }
        public required bool conversion {  get; set; }
    }
}
