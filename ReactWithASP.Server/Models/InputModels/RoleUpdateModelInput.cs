using System.ComponentModel.DataAnnotations;

namespace ReactWithASP.Server.Models.InputModels
{
    public class RoleUpdateModelInput
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email for account to update required.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Role to update to is required.")]
        public required string Role { get; set; }
    }
}
