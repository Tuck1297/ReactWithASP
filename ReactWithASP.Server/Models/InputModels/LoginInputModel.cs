using System.ComponentModel.DataAnnotations;

namespace ReactWithASP.Server.Models.InputModels
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string PasswordHash { get; set; }
    }
}
