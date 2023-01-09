namespace DominoAPI.Entities.Fleet
{
    public class FuelNote
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float Volume { get; set; }

        public int CarId { get; set; }
        public virtual Car? Car { get; set; }

        public int FuelSupplyId { get; set; }
        public virtual FuelSupply FuelSupply { get; set; }
    }
}