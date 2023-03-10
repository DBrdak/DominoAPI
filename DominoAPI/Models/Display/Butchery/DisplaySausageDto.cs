using DominoAPI.Entities.Butchery;
using DominoAPI.Entities;

namespace DominoAPI.Models.Display.Butchery
{
    public class DisplaySausageDto
    {
        public string Name { get; set; }
        public IList<DisplayIngredientDto> Ingredients { get; set; }
        public float Yield { get; set; }
        public float Price { get; set; }
    }
}