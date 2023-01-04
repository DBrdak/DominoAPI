using DominoAPI.Entities.Fleet;
using DominoAPI.Entities.Shops;

namespace DominoAPI.Models.Display.Shops
{
    public class DisplayShopDetailsDto
    {
        public int ShopNumber { get; set; }
        public string TypeOfShop { get; set; }

        public string? Address { get; set; }
        public string? RegistrationNumber { get; set; }

        public IEnumerable<DisplaySaleDto>? Sales { get; set; }
    }
}