using DominoAPI.Entities.Shops;

namespace DominoAPI.Models.Update.Shops
{
    public class UpdateShopDto
    {
        public int? ShopNumber { get; set; }
        public int? CarId { get; set; }
        public string? Address { get; set; }
    }
}