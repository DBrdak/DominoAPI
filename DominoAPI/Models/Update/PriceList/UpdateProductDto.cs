using DominoAPI.Entities.PriceList;

namespace DominoAPI.Models.Update.PriceList
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public ProductType? ProductType { get; set; }
        public float? Price { get; set; }
    }
}