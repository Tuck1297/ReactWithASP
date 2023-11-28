using System.ComponentModel.DataAnnotations;

namespace ReactWithASP.Server.Models.InputModels
{
    public class RegisterInputModel
    {
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(250, ErrorMessage = "First name cannot be longer than 250 characters.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(250, ErrorMessage = "Last name cannot be longer than 250 characters.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!#?]).{8,}$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character (!, #, ?).")]
        public required string PasswordHash { get; set; }

        [Required(ErrorMessage = "Confirmed password is required.")]
        [Compare("PasswordHash", ErrorMessage = "Password and confirmed password do not match.")]
        public required string ConfirmedPasswordHash { get; set; }
    }
}
