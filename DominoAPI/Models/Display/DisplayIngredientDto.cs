using DominoAPI.Entities;

namespace DominoAPI.Models
{
    public class DisplayIngredientDto
    {
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public float Content { get; set; }
    }
}