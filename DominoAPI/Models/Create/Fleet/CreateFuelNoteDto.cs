using System.ComponentModel.DataAnnotations;

namespace DominoAPI.Models.Create.Fleet
{
    public class CreateFuelNoteDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public float Volume { get; set; }

        [Required]
        public int CarId { get; set; }
    }
}