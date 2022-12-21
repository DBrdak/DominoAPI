namespace DominoAPI.Entities.Shops
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float SaleAmount { get; set; }
        public int Bills { get; set; }

        public int ShopId { get; set; }
        public virtual Shop Shop { get; set; }
    }
}