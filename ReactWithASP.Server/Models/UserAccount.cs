namespace ReactWithASP.Server.Models
{
    public class UserAccount
    {
        public Guid UserId { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenExpires { get; set; }
        public DateTime TokenCreated { get; set; }

    }
}
