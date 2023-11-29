using System.ComponentModel.DataAnnotations;

namespace ReactWithASP.Server.Models.OutputModels
{
    public class ConnectionStringOutputModel
    {
        public required string dbName { get; set; }

        public required string dbType { get; set; }
    }
}
