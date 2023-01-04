using AutoMapper;
using DominoAPI.Entities;
using DominoAPI.Entities.Butchery;
using DominoAPI.Entities.PriceList;
using DominoAPI.Models.Create;
using DominoAPI.Models.Display.Butchery;
using DominoAPI.Models.Update.Butchery;
using Microsoft.EntityFrameworkCore;

namespace DominoAPI.Services
{
    public interface IButcheryService
    {
        Task<List<DisplaySausageDto>> GetAllSausages();

        Task<DisplaySausageDto> GetSausage(int sausageId);

        Task<List<DisplayIngredientDto>> GetIngredients(int sausageId);

        Task AddSausage(CreateSausageDto dto);

        Task DeleteSausage(int sausageId);

        Task UpdateSausage(int sausageId, UpdateSausageDto dto);
    }

    public class ButcheryService : IButcheryService
    {
        private readonly DominoDbContext _dbContext;
        private readonly IMapper _mapper;

        public ButcheryService(DominoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<DisplaySausageDto>> GetAllSausages()
        {
            var sausages = await _dbContext.Sausages
                .Include(s => s.Product)
                .Include(s => s.Ingredients)
                .ThenInclude(i => i.Product)
                .AsNoTracking()
                .ToListAsync();

            var sausageDtos = _mapper.Map<List<DisplaySausageDto>>(sausages);

            return sausageDtos;
        }

        public async Task<DisplaySausageDto> GetSausage(int sausageId)
        {
            var sausage = await _dbContext.Sausages
                .Include(s => s.Product)
                .Include(s => s.Ingredients)
                .ThenInclude(i => i.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sausageId);

            if (sausage is null)
            {
                throw new Exception();
            }

            var sausageDto = _mapper.Map<DisplaySausageDto>(sausage);

            return sausageDto;
        }

        public async Task<List<DisplayIngredientDto>> GetIngredients(int sausageId)
        {
            var sausage = await _dbContext.Sausages
                .Include(s => s.Ingredients)
                .ThenInclude(i => i.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sausageId);

            if (sausage is null)
            {
                throw new Exception();
            }

            var ingredientDtos = _mapper.Map<List<DisplayIngredientDto>>(sausage.Ingredients);

            return ingredientDtos;
        }

        public async Task AddSausage(CreateSausageDto dto)
        {
            var product = await _dbContext.Products
                .Include(p => p.Sausage)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

            if (product is null
                || product.Sausage != null
                || product.ProductType != ProductType.Sausage
                || (int)dto.Ingredients.Select(i => i.Content).Sum() != 1)
            {
                throw new Exception();
            }

            foreach (var ingredient in dto.Ingredients)
            {
                var tempProduct = await _dbContext.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == ingredient.ProductId);

                if (tempProduct is null)
                {
                    throw new Exception();
                }
            }

            var newIngredients = _mapper.Map<List<Ingredient>>(dto.Ingredients);

            var newSausage = _mapper.Map<Sausage>(dto);

            await _dbContext.Sausages.AddAsync(newSausage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSausage(int sausageId)
        {
            var sausage = await _dbContext.Sausages
                .Include(s => s.Ingredients)
                .FirstOrDefaultAsync(s => s.Id == sausageId);

            if (sausage is null)
            {
                throw new Exception();
            }

            _dbContext.Sausages.Remove(sausage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateSausage(int sausageId, UpdateSausageDto dto)
        {
            var sausage = await _dbContext.Sausages
                .Include(s => s.Ingredients)
                .FirstOrDefaultAsync(s => s.Id == sausageId);

            if (sausage is null)
            {
                throw new Exception();
            }

            if (dto.Yield != null)
            {
                sausage.Yield = (float)dto.Yield;
            }

            if (dto.Ingredients != null &&
                (int)dto.Ingredients.Select(i => i.Content).Sum() == 1)
            {
                _dbContext.Ingredients.RemoveRange(sausage.Ingredients);

                foreach (var ingredient in dto.Ingredients)
                {
                    var product = await _dbContext.Products
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == ingredient.ProductId);

                    if (product is null)
                    {
                        throw new Exception();
                    }

                    ingredient.Product = product;
                }
            }

            var newIngredients = _mapper.Map<List<Ingredient>>(dto.Ingredients);

            sausage.Ingredients = newIngredients;

            _dbContext.Sausages.Update(sausage);
            await _dbContext.SaveChangesAsync();
        }
    }
}