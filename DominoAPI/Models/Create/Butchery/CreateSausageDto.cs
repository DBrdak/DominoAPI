using DominoAPI.Entities.Butchery;
using DominoAPI.Entities.PriceList;

namespace DominoAPI.Models.Create
{
    public class CreateSausageDto
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public List<CreateIngredientDto> Ingredients { get; set; }
        public float yield { get; set; }
    }
}