namespace DominoAPI.Models.Display.Shops
{
    public class DisplayStationaryShopDto : DisplayShopDto
    {
        public string Address { get; set; }
        public List<DisplaySaleDto> Sales { get; set; }
    }
}