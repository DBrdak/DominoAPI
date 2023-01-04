namespace DominoAPI.Entities.Fleet
{
    public class FuelSupply
    {
        public int Id { get; set; }
        public DateTime DateOfDelivery { get; set; }
        public float DeliveryVolume { get; set; }
        public float CurrentVolume { get; set; }
        public float Price { get; set; }

        public IEnumerable<FuelNote>? FuelNotes { get; set; }
    }
}