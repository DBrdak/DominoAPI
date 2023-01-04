namespace DominoAPI.Models.Update.Fleet
{
    public class UpdateFuelSupplyDto
    {
        public DateTime? DateOfDelivery { get; set; }
        public float? DeliveryVolume { get; set; }
        public float? Price { get; set; }
    }
}