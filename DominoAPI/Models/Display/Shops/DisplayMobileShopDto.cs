using DominoAPI.Entities.Fleet;
using DominoAPI.Entities.Shops;

namespace DominoAPI.Models.Display.Shops
{
    public class DisplayMobileShopDto : DisplayShopDto
    {
        public string RegistrationNumber { get; set; }
        public List<DisplaySaleDto> Sales { get; set; }
    }
}