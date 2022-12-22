using DominoAPI.Entities;
using DominoAPI.Entities.Butchery;
using Microsoft.IdentityModel.Tokens;

namespace DominoAPI
{
    public class Seeder
    {
        public static void Seed(DominoDbContext _dbContext)
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Sausages.Any() && !_dbContext.Products.Any())
                {
                    var products = new List<Product>()
                    {
                        new Product()
                        {
                            Name = "Kiełbasa krucha",
                            Price = 22.9f,
                            ProductType = ProductType.Sausage
                        },
                        new Product()
                        {
                            Name = "50/50",
                            Price = 7.9f,
                            ProductType = ProductType.Meat
                        },
                        new Product()
                        {
                            Name = "80/20",
                            Price = 14.9f,
                            ProductType = ProductType.Meat
                        }
                    };

                    _dbContext.Products.AddRange(products);

                    var sausage = new Sausage()
                    {
                        Ingredients = new List<Ingredient>()
                        {
                            new Ingredient()
                            {
                                Product = products[1],
                                Content = .3f
                            },
                            new Ingredient()
                            {
                                Product = products[2],
                                Content = .7f
                            }
                        },
                        Yield = 0.95f,
                        Product = products[0]
                    };
                    _dbContext.Sausages.Add(sausage);
                }
                _dbContext.SaveChanges();
            }
        }
    }
}