using DominoAPI.Entities.Shops;
using System.ComponentModel.DataAnnotations;

namespace DominoAPI.Models.Create.Shops
{
    public class CreateShopDto
    {
        [Required]
        public int ShopNumber { get; set; }

        public int? CarId { get; set; }
        public string? Address { get; set; }
    }
}