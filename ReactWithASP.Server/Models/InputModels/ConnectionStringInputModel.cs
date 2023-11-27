using System.ComponentModel.DataAnnotations;

namespace ReactWithASP.Server.Models.InputModels
{
    public class ConnectionStringInputModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Database Connection String required.")]
        public string dbEncryptedConnectionString { get; set; }

        [Required(ErrorMessage = "Database Connection Name is required.")]
        public string dbName { get; set; }

        [Required(ErrorMessage = "Database Type is required.")]
        public string dbType { get; set; }

        [Required(ErrorMessage = "User's ID that this connection belongs to is required.")]
        public Guid UserId { get; set; }
    }
}
