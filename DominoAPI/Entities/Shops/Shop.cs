using DominoAPI.Entities.Fleet;

namespace DominoAPI.Entities.Shops
{
    public class Shop
    {
        public int Id { get; set; }
        public int ShopNumber { get; set; }

        public IEnumerable<Sale>? Sales { get; set; }
    }
}