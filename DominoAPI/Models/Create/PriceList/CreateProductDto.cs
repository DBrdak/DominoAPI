using DominoAPI.Entities.PriceList;
using System.ComponentModel.DataAnnotations;

namespace DominoAPI.Models.Create.PriceList
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public ProductType ProductType { get; set; }
    }
}