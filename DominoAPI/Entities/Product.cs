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
    }
}