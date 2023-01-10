using System.ComponentModel.DataAnnotations;

namespace DominoAPI.Models.AccountModels.Display
{
    public class DisplayUserDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}