using System.ComponentModel.DataAnnotations;
using DominoAPI.Entities.Butchery;
using DominoAPI.Entities.PriceList;

namespace DominoAPI.Models.Create
{
    public class CreateSausageDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public IList<CreateIngredientDto> Ingredients { get; set; }

        [Required]
        public float yield { get; set; }
    }
}