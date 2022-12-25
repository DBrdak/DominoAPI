using DominoAPI.Entities;
using DominoAPI.Entities.Butchery;

namespace DominoAPI.Models.Create
{
    public class CreateSausageDto
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<CreateIngredientDto> Ingredients { get; set; }
        public float yield { get; set; }
    }
}