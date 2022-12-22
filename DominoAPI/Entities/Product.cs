using DominoAPI.Entities;
using DominoAPI.Entities.Butchery;

namespace DominoAPI.Entities
{
    public enum ProductType
    {
        Meat,
        Sausage,
        Spice
    }

    public class Product
    {
        public int Id { get; set; }
        public ProductType ProductType { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

        public virtual Sausage? Sausage { get; set; }
        public virtual Ingredient? Ingredient { get; set; }
    }
}