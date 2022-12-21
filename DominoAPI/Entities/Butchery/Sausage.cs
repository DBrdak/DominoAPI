﻿namespace DominoAPI.Entities.Butchery;

public class Sausage
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public float Yield { get; set; }

    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
}