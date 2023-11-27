using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ReactWithASP.Server.Models.InputModels
{
    public class UpdateUserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(250, ErrorMessage = "First name cannot be longer than 250 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(250, ErrorMessage = "Last name cannot be longer than 250 characters.")]
        public string LastName { get; set; }

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!#?]).{8,}$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character (!, #, ?).")]
        public string PasswordHash { get; set; }

        [Compare("PasswordHash", ErrorMessage = "Password and confirmed password do not match.")]
        public string ConfirmedPasswordHash { get; set; }
    }
}
