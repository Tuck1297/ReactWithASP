namespace ReactWithASP.Server.Models.TestDbs.WebsiteUsers
{
    public class Website1Users
    {
        public required Guid id { get; set; } = Guid.NewGuid();
        public required string username { get; set; }
        public required string password { get; set; }
        public required string email { get; set; }
        public required int age { get; set; }
        public required string gender { get; set; }
        public required string country { get; set; }
    }
}
