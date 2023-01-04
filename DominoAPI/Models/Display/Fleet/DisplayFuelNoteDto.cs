using DominoAPI.Entities.Fleet;

namespace DominoAPI.Models.Display.Fleet
{
    public class DisplayFuelNoteDto
    {
        public string Date { get; set; }
        public string RegistrationNumber { get; set; }
        public float Volume { get; set; }
    }
}