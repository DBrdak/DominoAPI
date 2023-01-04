using DominoAPI.Entities.Fleet;

namespace DominoAPI.Models.Update.Fleet
{
    public class UpdateFuelNoteDto
    {
        public DateTime? Date { get; set; }
        public float? Volume { get; set; }
        public int? CarId { get; set; }
    }
}