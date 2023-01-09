using System.ComponentModel.DataAnnotations;

namespace DominoAPI.Models.AccountModels
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        [MinLength(8)]
        public string? Password { get; set; }

        public string? PreviousPassword { get; set; }
    }
}