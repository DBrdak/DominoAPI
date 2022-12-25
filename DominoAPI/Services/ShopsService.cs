using AutoMapper;
using DominoAPI.Entities;
using DominoAPI.Models.Display;

namespace DominoAPI.Services
{
    public interface IShopsService
    {
    }

    public class ShopsService : IShopsService
    {
        private readonly DominoDbContext _dbContext;
        private readonly IMapper _mapper;

        public ShopsService(DominoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        //public async Task<List<DisplayShopDto>> GetAllShops()
        //{

        //}
    }
}
