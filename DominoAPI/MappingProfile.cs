using AutoMapper;
using DominoAPI.Entities;
using DominoAPI.Entities.Butchery;
using DominoAPI.Models;
using DominoAPI.Models.Create;
using DominoAPI.Models.Update;

namespace DominoAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, DisplayProductDto>()
                .ForMember(p => p.ProductType, a => a.MapFrom(b => b.ProductType.ToString()));

            CreateMap<CreateProductDto, Product>();

            CreateMap<Ingredient, DisplayIngredientDto>()
                .ForMember(i => i.ProductName, a => a.MapFrom(b => b.Product.Name))
                .ForMember(i => i.ProductType, a => a.MapFrom(b => b.Product.ProductType.ToString()));

            CreateMap<CreateIngredientDto, Ingredient>()
                .ForMember(i => i.ProductId, a => a.Ignore());

            CreateMap<Sausage, DisplaySausageDto>()
                .ForMember(s => s.Name, a => a.MapFrom(b => b.Product.Name))
                .ForMember(s => s.Price, a => a.MapFrom(b => b.Product.Price))
                .ForMember(s => s.Ingredients, a => a.MapFrom(b => b.Ingredients));
        }
    }
}