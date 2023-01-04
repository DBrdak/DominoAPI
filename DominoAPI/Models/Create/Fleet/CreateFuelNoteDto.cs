namespace DominoAPI.Models.Create.Fleet
{
    public class CreateFuelNoteDto
    {
        public DateTime Date { get; set; }
        public float Volume { get; set; }
        public int CarId { get; set; }
    }
}