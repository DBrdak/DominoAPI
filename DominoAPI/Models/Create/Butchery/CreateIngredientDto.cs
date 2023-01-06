using System.ComponentModel.DataAnnotations;
using DominoAPI.Entities.PriceList;

namespace DominoAPI.Models.Create
{
    public class CreateIngredientDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public float Content { get; set; }
    }
}