using DominoAPI.Entities.Shops;

namespace DominoAPI.Models.Create.Shops
{
    public class CreateShopDto
    {
        public int ShopNumber { get; set; }
        public TypeofShop TypeOfShop { get; set; }

        public int? CarId { get; set; }
        public string? Address { get; set; }
    }
}