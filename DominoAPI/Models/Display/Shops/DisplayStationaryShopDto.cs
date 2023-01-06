namespace DominoAPI.Models.Display.Shops
{
    public class DisplayStationaryShopDto : DisplayShopDto
    {
        public string Address { get; set; }
        public IList<DisplaySaleDto> Sales { get; set; }
    }
}