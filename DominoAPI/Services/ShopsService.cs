using AutoMapper;
using DominoAPI.Entities;
using DominoAPI.Entities.Shops;
using DominoAPI.Models.Create.Shops;
using DominoAPI.Models.Display.Shops;
using DominoAPI.Models.Update.Shops;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using UtilityLibrary;

namespace DominoAPI.Services
{
    public interface IShopsService
    {
        Task<List<DisplayShopDto>> GetAllShops();

        Task<object> GetShopDetails(int shopId);

        Task<IEnumerable<DisplaySaleDto>> GetSales(int shopId);

        Task AddShop(CreateShopDto dto);

        Task AddNewSale(CreateSaleDto dto, int shopId);

        Task AddNewRecentSale(CreateSaleDto dto, int shopId);

        Task UpdateShop(UpdateShopDto dto, int shopId);

        Task DeleteShop(int shopId);

        Task DeleteSale(int shopId, int saleId);

        Task DeleteSalesRange(int shopId, IEnumerable<int> salesId);

        Task DeleteAllSales(int shopId);
    }

    public class ShopsService : IShopsService
    {
        private readonly DominoDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ShopsService> _logger;

        public ShopsService(DominoDbContext dbContext, IMapper mapper, ILogger<ShopsService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<DisplayShopDto>> GetAllShops()
        {
            var mobileShops = await _dbContext.MobileShops
                .AsNoTracking()
                .ToListAsync();

            var stationaryShops = await _dbContext.StationaryShops
                .AsNoTracking()
                .ToListAsync();

            var dto = _mapper.Map<List<DisplayShopDto>>(mobileShops);
            dto.AddRange(_mapper.Map<List<DisplayShopDto>>(stationaryShops));

            return dto;
        }

        public async Task<object> GetShopDetails(int shopId)
        {
            var mobileShop = await _dbContext.MobileShops
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == shopId);

            var stationaryShop = await _dbContext.StationaryShops
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == shopId);

            var isMobile = stationaryShop is null;

            if (isMobile && !isMobile)
            {
                throw new Exception();
            }

            var dto = new object();

            if (isMobile) dto = _mapper.Map<DisplayMobileShopDto>(mobileShop);
            if (!isMobile) dto = _mapper.Map<DisplayStationaryShopDto>(stationaryShop);

            return dto;
        }

        public async Task<IEnumerable<DisplaySaleDto>> GetSales(int shopId)
        {
            var shop = await _dbContext.Shops
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop is null)
            {
                throw new Exception();
            }

            var sales = await _dbContext.Sales
                .AsNoTracking()
                .Where(ss => ss.ShopId == shopId)
                .ToListAsync();

            var dto = _mapper.Map<IEnumerable<DisplaySaleDto>>(sales);

            return dto;
        }

        public async Task AddShop(CreateShopDto dto)
        {
            var isMobile = dto.Address is null;

            if (isMobile && !isMobile)
            {
                throw new Exception();
            }

            var newShop = new object();

            if (isMobile) newShop = _mapper.Map<MobileShop>(dto);
            if (!isMobile) newShop = _mapper.Map<StationaryShop>(dto);

            await _dbContext.AddAsync(newShop);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddNewSale(CreateSaleDto dto, int shopId)
        {
            if (dto.Date > DateTime.Now)
            {
                throw new Exception();
            }

            var shop = await _dbContext.Shops
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop is null)
            {
                throw new Exception();
            }

            var newSale = _mapper.Map<Sale>(dto);

            newSale.ShopId = shopId;

            await _dbContext.Sales.AddAsync(newSale);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddNewRecentSale(CreateSaleDto dto, int shopId)
        {
            var shop = await _dbContext.Shops
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop is null)
            {
                throw new Exception();
            }

            dto.Date = DateTime.Now;

            var newSale = _mapper.Map<Sale>(dto);

            newSale.ShopId = shopId;

            await _dbContext.Sales.AddAsync(newSale);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateShop(UpdateShopDto dto, int shopId)
        {
            var shop = new object();

            shop = await _dbContext.MobileShops
                .FirstOrDefaultAsync(s => s.Id == shopId);
            var isMobile = true;

            if (shop is null)
            {
                shop = await _dbContext.StationaryShops
                    .FirstOrDefaultAsync(s => s.Id == shopId);
                isMobile = false;
            }

            if (shop is null)
            {
                throw new Exception();
            }

            if (isMobile)
            {
                dto.Address = null;
                dto.MapTo(shop);
            }
            else
            {
                dto.CarId = null;
                dto.MapTo(shop);
            }

            _dbContext.Update(shop);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteShop(int shopId)
        {
            var shop = await _dbContext.Shops
                //.Include(s => s.Sales)
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop is null)
            {
                throw new Exception();
            }

            _dbContext.Remove(shop);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSale(int shopId, int saleId)
        {
            var shop = await _dbContext.Shops
                .Include(s => s.Sales)
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop is null)
            {
                throw new Exception();
            }

            var sale = shop.Sales
                .FirstOrDefault(ss => ss.Id == saleId);

            if (sale is null)
            {
                throw new Exception();
            }

            _dbContext.Sales.Remove(sale);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSalesRange(int shopId, IEnumerable<int> salesId)
        {
            var shop = await _dbContext.Shops
                .Include(s => s.Sales)
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop == null)
            {
                throw new Exception();
            }

            var sales = new List<Sale>();

            foreach (var saleId in salesId)
            {
                var sale = shop.Sales
                    .FirstOrDefault(ss => ss.Id == saleId);

                if (sale is null)
                {
                    throw new Exception();
                }

                sales.Add(sale);
            }

            _dbContext.Sales.RemoveRange(sales);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAllSales(int shopId)
        {
            var shop = await _dbContext.Shops
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop is null)
            {
                throw new Exception();
            }

            var sales = await _dbContext.Sales
                .Where(ss => ss.ShopId == shopId)
                .ToListAsync();

            _dbContext.Sales.RemoveRange(sales);
            await _dbContext.SaveChangesAsync();
        }
    }
}