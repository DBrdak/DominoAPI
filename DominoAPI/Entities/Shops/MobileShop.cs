using DominoAPI.Entities.Fleet;

namespace DominoAPI.Entities.Shops
{
    public class MobileShop : Shop
    {
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public readonly string TypeOfShop = "Mobile";
    }
}