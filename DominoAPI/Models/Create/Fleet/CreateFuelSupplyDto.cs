namespace DominoAPI.Models.Create.Fleet
{
    public class CreateFuelSupplyDto
    {
        public DateTime DateOfDelivery { get; set; }
        public int DeliveryVolume { get; set; }
        public float Price { get; set; }
    }
}