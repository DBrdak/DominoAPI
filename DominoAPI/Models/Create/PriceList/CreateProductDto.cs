using DominoAPI.Entities.PriceList;

namespace DominoAPI.Models.Create.PriceList
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public ProductType ProductType { get; set; }
    }
}