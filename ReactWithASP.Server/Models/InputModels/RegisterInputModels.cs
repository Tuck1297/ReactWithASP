using System.ComponentModel.DataAnnotations;

namespace ReactWithASP.Server.Models.InputModels
{
    public class RegisterInputModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(250, ErrorMessage = "First name cannot be longer than 250 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(250, ErrorMessage = "Last name cannot be longer than 250 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!#?]).{8,}$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character (!, #, ?).")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Confirmed password is required.")]
        [Compare("PasswordHash", ErrorMessage = "Password and confirmed password do not match.")]
        public string ConfirmedPasswordHash { get; set; }
    }
}
