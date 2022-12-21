namespace DominoAPI.Entities.Butchery
{
    public class Ingredient
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public float Content { get; set; }
    }
}