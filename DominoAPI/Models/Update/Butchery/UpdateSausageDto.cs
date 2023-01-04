using DominoAPI.Models.Create;

namespace DominoAPI.Models.Update.Butchery
{
    public class UpdateSausageDto
    {
        public float? Yield { get; set; }
        public List<CreateIngredientDto>? Ingredients { get; set; }
    }
}