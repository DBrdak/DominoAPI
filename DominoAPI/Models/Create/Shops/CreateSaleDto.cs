namespace DominoAPI.Models.Create.Shops
{
    public class CreateSaleDto
    {
        public DateTime Date { get; set; }
        public float SaleAmount { get; set; }
        public int Bills { get; set; }
    }
}