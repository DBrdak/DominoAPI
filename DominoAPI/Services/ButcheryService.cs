using AutoMapper;
using DominoAPI.Entities;
using DominoAPI.Entities.Butchery;
using DominoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DominoAPI.Services
{
    public interface IButcheryService
    {
        Task<List<DisplaySausageDto>> GetAllSausages();

        Task<DisplaySausageDto> GetSausage(int sausageId);

        Task<List<DisplayIngredientDto>> GetIngredients(int sausageId);
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
    }
}