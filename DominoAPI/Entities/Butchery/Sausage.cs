using DominoAPI.Entities.PriceList;

namespace DominoAPI.Entities.Butchery;

public class Sausage
{
    public int Id { get; set; }
    public float Yield { get; set; }

    public IList<Ingredient> Ingredients { get; set; }

    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
}