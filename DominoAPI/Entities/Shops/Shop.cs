namespace DominoAPI.Entities.Shops
{
    public enum TypeofShop
    {
        Mobile,
        Stationary
    }

    public class Shop
    {
        public int Id { get; set; }
        public int ShopNumber { get; set; }
        public TypeofShop TypeOfShop { get; set; }
        public IEnumerable<Sale> Sales { get; set; }
    }
}