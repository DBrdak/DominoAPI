using DominoAPI.Entities.Fleet;

namespace DominoAPI.Models.Display.Fleet
{
    public class DisplayFuelSupplyDto
    {
        public string DateOfDelivery { get; set; }
        public float DeliveryVolume { get; set; }
        public float CurrentVolume { get; set; }
        public float Price { get; set; }

        public IEnumerable<DisplayFuelNoteDto>? FuelNotes { get; set; }
    }
}