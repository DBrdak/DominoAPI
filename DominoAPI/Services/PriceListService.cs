using AutoMapper;
using DominoAPI.Entities;
using DominoAPI.Entities.Butchery;
using DominoAPI.Entities.PriceList;
using DominoAPI.Models.Create.PriceList;
using DominoAPI.Models.Display.Fleet;
using DominoAPI.Models.Update.PriceList;
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

            var dto = _mapper.Map<List<DisplayProductDto>>(products);

            return dto;
        }

        public async Task AddProduct(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(UpdateProductDto dto, int id)
        {
            var product = await _dbContext.Products
                .Include(p => p.Ingredient)
                .Include(p => p.Sausage)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                throw new Exception();
            }

            if (dto.Name is not null)
            {
                product.Name = dto.Name;
            }
            if (dto.Price is not null)
            {
                product.Price = (float)dto.Price;
            }
            if (dto.ProductType is not null && product.Sausage is null && !product.Ingredient.Any())
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
                .ThenInclude(s => s.Ingredients)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (product is null)
            {
                throw new Exception();
            }

            if (!product.Ingredient.Any())
            {
                throw new Exception();
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}