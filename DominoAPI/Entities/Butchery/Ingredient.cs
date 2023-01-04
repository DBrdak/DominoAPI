using DominoAPI.Entities.PriceList;

namespace DominoAPI.Entities.Butchery
{
    public class Ingredient
    {
        public int Id { get; set; }
        public float Content { get; set; }

        public int SausageId { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}