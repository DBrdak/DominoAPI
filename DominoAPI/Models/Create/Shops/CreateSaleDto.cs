using System.ComponentModel.DataAnnotations;

namespace DominoAPI.Models.Create.Shops
{
    public class CreateSaleDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public float SaleAmount { get; set; }

        [Required]
        public int Bills { get; set; }
    }
}