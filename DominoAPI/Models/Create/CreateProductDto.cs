using DominoAPI.Entities;

namespace DominoAPI.Models.Create
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public ProductType ProductType { get; set; }
    }
}
