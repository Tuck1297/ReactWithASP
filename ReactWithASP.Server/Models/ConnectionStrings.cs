namespace ReactWithASP.Server.Models
{
    public class ConnectionStrings
    {
        public required Guid Id { get; set; }
        public required string dbName { get; set; }
        public required string dbType { get; set; }
        public required string dbEncryptedConnectionString { get; set; }
        public required Guid UserId { get; set; }
        public User User { get; set; }
    }
}
