using System.ComponentModel.DataAnnotations;

namespace DominoAPI.Models.Create.Fleet
{
    public class CreateCarDto
    {
        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [MaxLength(30)]
        public string? Description { get; set; }

        [MaxLength(30)]
        public string? Note { get; set; }

        public int? Mileage { get; set; }
    }
}