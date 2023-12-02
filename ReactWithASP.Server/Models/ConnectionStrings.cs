using Microsoft.EntityFrameworkCore;

namespace ReactWithASP.Server.Models
{
    public class ConnectionStrings
    {
        public required Guid Id { get; set; }
        
        public required string dbName { get; set; }
        public required string dbType { get; set; }
        public required string dbConnectionString { get; set; }
        public required Guid UserId { get; set; }
        public string currentTableInteracting {  get; set; } = string.Empty;
        public User User { get; set; }
    }
}
