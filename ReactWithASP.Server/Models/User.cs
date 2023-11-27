using System;

namespace ReactWithASP.Server.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public ICollection<ConnectionStrings> ConnectionStrings { get; set; }
    }
}
