namespace ReactWithASP.Server.Models
{
    public class ConnectionStrings
    {
        public Guid Id { get; set; }
        public string dbName { get; set; }
        public string dbType { get; set; }
        public string dbEncryptedConnectionString { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
