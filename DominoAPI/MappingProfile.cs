using AutoMapper;
using DominoAPI.Entities.Accounts;
using DominoAPI.Entities.Butchery;
using DominoAPI.Entities.Fleet;
using DominoAPI.Entities.PriceList;
using DominoAPI.Entities.Shops;
using DominoAPI.Models.AccountModels;
using DominoAPI.Models.AccountModels.Display;
using DominoAPI.Models.Create;
using DominoAPI.Models.Create.Fleet;
using DominoAPI.Models.Create.PriceList;
using DominoAPI.Models.Create.Shops;
using DominoAPI.Models.Display.Butchery;
using DominoAPI.Models.Display.Fleet;
using DominoAPI.Models.Display.Shops;
using DominoAPI.Models.Update;

namespace DominoAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // PriceList maps
            {
                CreateMap<Product, DisplayProductDto>()
                    .ForMember(p => p.ProductType, a => a.MapFrom(b => b.ProductType.ToString()));

                CreateMap<CreateProductDto, Product>();
            }

            // Butchery maps
            {
                CreateMap<Ingredient, DisplayIngredientDto>()
                    .ForMember(i => i.ProductName, a => a.MapFrom(b => b.Product.Name))
                    .ForMember(i => i.ProductType, a => a.MapFrom(b => b.Product.ProductType.ToString()));

                CreateMap<CreateIngredientDto, Ingredient>();

                CreateMap<CreateSausageDto, Sausage>();

                CreateMap<Sausage, DisplaySausageDto>()
                    .ForMember(s => s.Name, a => a.MapFrom(b => b.Product.Name))
                    .ForMember(s => s.Price, a => a.MapFrom(b => b.Product.Price))
                    .ForMember(s => s.Ingredients, a => a.MapFrom(b => b.Ingredients));
            }

            // Shop maps
            {
                CreateMap<MobileShop, DisplayShopDto>();
                CreateMap<StationaryShop, DisplayShopDto>();

                CreateMap<Sale, DisplaySaleDto>()
                    .ForMember(ss => ss.Date, a => a.MapFrom(b => b.Date.ToShortDateString()));

                CreateMap<MobileShop, DisplayMobileShopDto>()
                    .ForMember(s => s.Sales, a => a.MapFrom(b => b.Sales));

                CreateMap<StationaryShop, DisplayStationaryShopDto>()
                    .ForMember(s => s.Sales, a => a.MapFrom(b => b.Sales));

                CreateMap<CreateShopDto, MobileShop>();
                CreateMap<CreateShopDto, StationaryShop>();

                CreateMap<CreateSaleDto, Sale>();
            }

            // Fleet maps
            {
                CreateMap<Car, DisplayCarDto>()
                    .ForMember(c => c.ShopNumber, a => a.MapFrom(b => b.Shop.ShopNumber));

                CreateMap<FuelNote, DisplayFuelNoteDto>()
                    .ForMember(fn => fn.Date, a => a.MapFrom(b => b.Date.ToShortDateString()))
                    .ForMember(fn => fn.RegistrationNumber, a => a.MapFrom(b => b.Car.RegistrationNumber));

                CreateMap<FuelSupply, DisplayFuelSupplyDto>()
                    .ForMember(fs => fs.DateOfDelivery, a => a.MapFrom(b => b.DateOfDelivery.ToShortDateString()))
                    .ForMember(fs => fs.FuelNotes, a => a.MapFrom(b => b.FuelNotes));

                CreateMap<CreateCarDto, Car>();

                CreateMap<CreateFuelNoteDto, FuelNote>();

                CreateMap<CreateFuelSupplyDto, FuelSupply>()
                    .ForMember(fs => fs.CurrentVolume, a => a.MapFrom(b => b.DeliveryVolume));
            }

            //Account maps
            {
                CreateMap<RegisterUserDto, User>();

                CreateMap<User, DisplayUserDto>();
            }
        }
    }
}