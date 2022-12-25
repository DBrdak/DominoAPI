using AutoMapper;
using DominoAPI.Entities;
using DominoAPI.Entities.Butchery;
using DominoAPI.Models;
using DominoAPI.Models.Create;
using Microsoft.EntityFrameworkCore;

namespace DominoAPI.Services
{
    public interface IPriceListService
    {
        Task<List<DisplayProductDto>> GetAllProducts(ProductType productType);

        Task AddProduct(CreateProductDto dto);

        Task UpdateProduct(UpdateProductDto dto, int id);

        Task DeleteProduct(int Id);
    }

    public class PriceListService : IPriceListService
    {
        private readonly DominoDbContext _dbContext;
        private readonly IMapper _mapper;

        public PriceListService(DominoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<DisplayProductDto>> GetAllProducts(ProductType productType)
        {
            var products = await _dbContext.Products
                .Where(p => p.ProductType == productType)
                .AsNoTracking()
                .ToListAsync();

            var productDtos = _mapper.Map<List<DisplayProductDto>>(products);

            productDtos.Select(p => p.ProductType);

            return productDtos;
        }

        public async Task AddProduct(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(UpdateProductDto dto, int id)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                throw new Exception();
            }

            if (dto.Name != null)
            {
                product.Name = dto.Name;
            }
            if (dto.Price != null)
            {
                product.Price = (float)dto.Price;
            }
            if (dto.ProductType != null && product.Sausage == null)
            {
                product.ProductType = (ProductType)dto.ProductType;
            }

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(int Id)
        {
            var product = await _dbContext.Products
                .Include(p => p.Ingredient)
                .Include(p => p.Sausage)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (product is null)
            {
                throw new Exception();
            }

            if (product.Ingredient.Count() == null || product.Sausage != null)
            {
                throw new Exception("Can't remove object that exists in other table");
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}