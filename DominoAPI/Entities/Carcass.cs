namespace DominoAPI.Entities.Variables
{
    public enum Part
    {
        Front,
        Middle,
        Back,
        Leg
    }

    public class Carcass
    {
        public int Id { get; set; }
        public Part Part { get; set; }
        public string Class { get; set; }
        public string ProductName { get; set; }
        public float ProductPercentage { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}