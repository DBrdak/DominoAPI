using DominoAPI.Entities.Shops;

namespace DominoAPI.Models.Update
{
    public class UpdateShopDto
    {
        public int? ShopNumber { get; set;}
        public TypeofShop? TypeofShop { get; set; }
        
        public string? Address { get; set; }
        public int? CarId { get; set; }
    }
}
