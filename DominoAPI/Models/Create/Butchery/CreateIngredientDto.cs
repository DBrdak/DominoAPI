using DominoAPI.Entities.PriceList;

namespace DominoAPI.Models.Create
{
    public class CreateIngredientDto
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public float Content { get; set; }
    }
}