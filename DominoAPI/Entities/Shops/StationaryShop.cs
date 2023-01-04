using Microsoft.Identity.Client;

namespace DominoAPI.Entities.Shops
{
    public class StationaryShop : Shop
    {
        public string Address { get; set; }
        public readonly string TypeOfShop = "Stationary";
    }
}