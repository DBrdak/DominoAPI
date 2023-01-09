using System.ComponentModel.DataAnnotations;

namespace DominoAPI.Models.AccountModels
{
    public class RegisterUserDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public readonly int RoleId = 4;
    }
}