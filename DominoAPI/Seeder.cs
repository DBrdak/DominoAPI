using System.Runtime.CompilerServices;
using DominoAPI.Entities;
using DominoAPI.Entities.Butchery;
using DominoAPI.Entities.Fleet;
using DominoAPI.Entities.PriceList;
using DominoAPI.Entities.Shops;
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
                                Content = 30
                            },
                            new Ingredient()
                            {
                                Product = products[2],
                                Content = 70
                            }
                        },
                        Yield = 0.95f,
                        Product = products[0]
                    };
                    _dbContext.Sausages.Add(sausage);
                }

                if (!_dbContext.FuelNotes.Any() && !_dbContext.FuelSupplies.Any() && !_dbContext.Cars.Any()
                    && !_dbContext.Shops.Any() && !_dbContext.Sales.Any())
                {
                    var cars = new List<Car>()
                    {
                        new Car()
                        {
                            Make = "Mercedes-Benz",
                            Model = "Sprinter",
                            Description = "Dostawczy",
                            Mileage = 321008,
                            Note = "Olej za 500 km",
                            RegistrationNumber = "WP5979N"
                        },
                        new Car()
                        {
                            Make = "Mercedes-Benz",
                            Model = "Sprinter",
                            Description = "Obwoźny Jarek",
                            Mileage = 513451,
                            Note = "Alternator do zrobienia",
                            RegistrationNumber = "WPNEG25"
                        },
                        new Car()
                        {
                            Make = "Mercedes-Benz",
                            Model = "Sprinter",
                            Description = "Obwoźny Darek",
                            Mileage = 498120,
                            RegistrationNumber = "WPNJG68"
                        },
                        new Car()
                        {
                            Make = "Mercedes-Benz",
                            Model = "Sprinter",
                            Description = "Obwoźny Sebastian",
                            Mileage = 432981,
                            RegistrationNumber = "WPNJG48"
                        },
                        new Car()
                        {
                        Make = "Mercedes-Benz",
                        Model = "Sprinter",
                        Description = "Obwoźny Zenek",
                        Mileage = 578123,
                        RegistrationNumber = "WPNJG58"
                        },
                        new Car()
                        {
                            Make = "Fiat",
                            Model = "Ducato",
                            Description = "Dostawczy",
                            Mileage = 176298,
                            Note = "Wymienić opony",
                            RegistrationNumber = "WGM54739"
                        },
                        new Car()
                        {
                            Make = "Fiat",
                            Model = "Fiorino",
                            Mileage = 209887,
                            RegistrationNumber = "WPNME11"
                        }
                    };

                    var shops = new List<Shop>()
                    {
                        new StationaryShop()
                        {
                            ShopNumber = 1,
                            Address = "Raciąż ul. Piłsudskiego 2/1"
                        },
                        new MobileShop()
                        {
                            ShopNumber = 2,
                            Car = cars[1]
                        },
                        new MobileShop()
                        {
                            ShopNumber = 4,
                            Car = cars[3]
                        },
                        new MobileShop()
                        {
                            ShopNumber = 5,
                            Car = cars[4]
                        },
                        new MobileShop()
                        {
                            ShopNumber = 6,
                            Car = cars[2]
                        },
                        new StationaryShop()
                        {
                            ShopNumber = 8,
                            Address = "Pólka-Raciąż 75A"
                        },
                    };

                    var sales = new List<Sale>()
                    {
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 4),
                            SaleAmount = 1192.93F,
                            Shop = shops[0]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 4),
                            SaleAmount = 9981.15F,
                            Shop = shops[1]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 4),
                            SaleAmount = 6409.29F,
                            Shop = shops[2]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 4),
                            SaleAmount = 5691.21F,
                            Shop = shops[3]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 4),
                            SaleAmount = 7091.32F,
                            Shop = shops[4]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 4),
                            SaleAmount = 10928.18F,
                            Shop = shops[5]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 5),
                            SaleAmount = 1872.98F,
                            Shop = shops[0]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 5),
                            SaleAmount = 4909.10F,
                            Shop = shops[1]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 5),
                            SaleAmount = 3591.21F,
                            Shop = shops[2]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 5),
                            SaleAmount = 3781.23F,
                            Shop = shops[3]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 5),
                            SaleAmount = 8891.21F,
                            Shop = shops[4]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 5),
                            SaleAmount = 6891.51F,
                            Shop = shops[5]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 6),
                            SaleAmount = 1982.67F,
                            Shop = shops[0]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 6),
                            SaleAmount = 7550.21F,
                            Shop = shops[1]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 6),
                            SaleAmount = 5581.09F,
                            Shop = shops[2]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 6),
                            SaleAmount = 3891.51F,
                            Shop = shops[3]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 6),
                            SaleAmount = 4491.29F,
                            Shop = shops[4]
                        },
                        new Sale()
                        {
                            Bills = 109,
                            Date = new DateTime(2022, 10, 6),
                            SaleAmount = 4998.23F,
                            Shop = shops[5]
                        }
                    };

                    var fuelNotes = new List<FuelNote>()
                    {
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 10, 03),
                            Volume = 52.5F,
                            Car = cars[0]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 10, 03),
                            Volume = 60.9F,
                            Car = cars[1]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 10, 05),
                            Volume = 52.2F,
                            Car = cars[2]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 10, 06),
                            Volume = 32.4F,
                            Car = cars[3]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 10, 07),
                            Volume = 70.4F,
                            Car = cars[4]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 10, 10),
                            Volume = 61.8F,
                            Car = cars[5]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 10, 10),
                            Volume = 80.2F,
                            Car = cars[0]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 10, 10),
                            Volume = 44.8F,
                            Car = cars[1]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 3),
                            Volume = 52.5F,
                            Car = cars[0]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 3),
                            Volume = 71.9F,
                            Car = cars[1]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 17),
                            Volume = 54.2F,
                            Car = cars[2]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 17),
                            Volume = 41.4F,
                            Car = cars[3]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 17),
                            Volume = 34.4F,
                            Car = cars[4]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 16),
                            Volume = 81.8F,
                            Car = cars[5]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 24),
                            Volume = 39.2F,
                            Car = cars[0]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 24),
                            Volume = 57.8F,
                            Car = cars[1]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 24),
                            Volume = 44.5F,
                            Car = cars[0]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 26),
                            Volume = 38.1F,
                            Car = cars[1]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 26),
                            Volume = 43.9F,
                            Car = cars[2]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 30),
                            Volume = 59.3F,
                            Car = cars[3]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 9),
                            Volume = 57.3F,
                            Car = cars[4]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 14),
                            Volume = 53.8F,
                            Car = cars[5]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 14),
                            Volume = 43.2F,
                            Car = cars[0]
                        },
                        new FuelNote()
                        {
                            Date = new DateTime(2022, 11, 14),
                            Volume = 53.8F,
                            Car = cars[1]
                        }
                    };

                    var fuelSupplies = new List<FuelSupply>()
                    {
                        new FuelSupply()
                        {
                            DateOfDelivery = new DateTime(2022, 10, 1),
                            Price = 7.49F,
                            DeliveryVolume = 2564,
                            FuelNotes = new List<FuelNote>()
                            {
                                fuelNotes[0],
                                fuelNotes[1],
                                fuelNotes[2],
                                fuelNotes[3],
                                fuelNotes[4],
                                fuelNotes[5],
                                fuelNotes[6],
                                fuelNotes[7]
                            }
                        },
                        new FuelSupply()
                        {
                            DateOfDelivery = new DateTime(2022, 11, 4),
                            Price = 7.45F,
                            DeliveryVolume = 2509,
                            FuelNotes = new List<FuelNote>()
                            {
                                fuelNotes[8],
                                fuelNotes[9],
                                fuelNotes[10],
                                fuelNotes[11],
                                fuelNotes[12],
                                fuelNotes[13],
                                fuelNotes[14],
                                fuelNotes[15],
                                fuelNotes[16],
                                fuelNotes[17],
                                fuelNotes[18],
                                fuelNotes[19],
                                fuelNotes[20],
                                fuelNotes[21],
                                fuelNotes[22],
                                fuelNotes[23]
                            }
                        }
                    };

                    _dbContext.Cars.AddRange(cars);
                    _dbContext.Shops.AddRange(shops);
                    _dbContext.Sales.AddRange(sales);
                    _dbContext.FuelNotes.AddRange(fuelNotes);
                    _dbContext.FuelSupplies.AddRange(fuelSupplies);
                }

                _dbContext.SaveChanges();
            }
        }
    }
}