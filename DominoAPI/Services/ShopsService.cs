using AutoMapper;
using DominoAPI.Entities;
using DominoAPI.Entities.Shops;
using DominoAPI.Models.Create.Shops;
using DominoAPI.Models.Display.Shops;
using DominoAPI.Models.Update;
using Microsoft.EntityFrameworkCore;

namespace DominoAPI.Services
{
    public interface IShopsService
    {
        Task<List<DisplayShopDto>> GetAllShops();

        Task<DisplayShopDetailsDto> GetShopDetails(int shopId);

        Task<IEnumerable<DisplaySaleDto>> GetSales(int shopId);

        Task AddShop(CreateShopDto dto);

        Task AddNewSale(CreateSaleDto dto, int shopId);

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

        public ShopsService(DominoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<DisplayShopDto>> GetAllShops()
        {
            var shops = await _dbContext.Shops
                .AsNoTracking()
                .ToListAsync();

            var dto = _mapper.Map<List<DisplayShopDto>>(shops);

            return dto;
        }

        public async Task<DisplayShopDetailsDto> GetShopDetails(int shopId)
        {
            var shop = await _dbContext.Shops
                .Include(s => s.Car)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop == null)
            {
                throw new Exception();
            }

            var shopDto = _mapper.Map<DisplayShopDetailsDto>(shop);

            return shopDto;
        }

        public async Task<IEnumerable<DisplaySaleDto>> GetSales(int shopId)
        {
            var shop = await _dbContext.Shops
                .Include(s => s.Car)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop == null)
            {
                throw new Exception();
            }

            var sales = await _dbContext.Sales
                .AsNoTracking()
                .Where(ss => ss.ShopId == shopId)
                .ToListAsync();

            var salesDto = _mapper.Map<IEnumerable<DisplaySaleDto>>(sales);

            return salesDto;
        }

        public async Task AddShop(CreateShopDto dto)
        {
            if (dto.TypeOfShop == TypeofShop.Mobile
                && dto.Address == null)
            {
                var car = await _dbContext.Cars
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == dto.CarId);

                if (car == null)
                {
                    throw new Exception();
                }

                var shop = new Shop()
                {
                    ShopNumber = dto.ShopNumber,
                    TypeOfShop = dto.TypeOfShop,
                    CarId = car.Id
                };

                await _dbContext.Shops.AddAsync(shop);
                await _dbContext.SaveChangesAsync();
            }
            else if (dto.TypeOfShop == TypeofShop.Stationary
                     && dto.CarId == null
                     && dto.Address != null)
            {
                var shop = new Shop()
                {
                    ShopNumber = dto.ShopNumber,
                    TypeOfShop = dto.TypeOfShop,
                    Address = dto.Address
                };

                await _dbContext.Shops.AddAsync(shop);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception();
            }
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

            if (shop == null)
            {
                throw new Exception();
            }

            var sale = new Sale()
            {
                Bills = dto.Bills,
                Date = dto.Date,
                SaleAmount = dto.SaleAmount,
                ShopId = shop.Id
            };

            await _dbContext.Sales.AddAsync(sale);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateShop(UpdateShopDto dto, int shopId)
        {
            var shop = await _dbContext.Shops
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop == null)
            {
                throw new Exception();
            }

            if (dto.TypeofShop == TypeofShop.Mobile && shop.TypeOfShop != TypeofShop.Mobile)
            {
                var car = await _dbContext.Cars
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == dto.CarId);

                if (car == null)
                {
                    throw new Exception();
                }

                shop.CarId = dto.CarId;
                shop.TypeOfShop = TypeofShop.Mobile;
            }
            else if (dto.TypeofShop == TypeofShop.Stationary && shop.TypeOfShop != TypeofShop.Stationary)
            {
                shop.Address = dto.Address;
                shop.TypeOfShop = TypeofShop.Stationary;
            }
            else if (shop.TypeOfShop == TypeofShop.Mobile && dto.CarId != null)
            {
                var car = await _dbContext.Cars
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == dto.CarId);

                if (car == null)
                {
                    throw new Exception();
                }

                shop.CarId = dto.CarId;
            }
            else if (shop.TypeOfShop == TypeofShop.Stationary && dto.Address != null)
            {
                shop.Address = dto.Address;
            }

            if (dto.ShopNumber != null)
            {
                shop.ShopNumber = (int)dto.ShopNumber;
            }

            _dbContext.Update(shop);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteShop(int shopId)
        {
            var shop = await _dbContext.Shops
                .Include(s => s.Sales)
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop == null)
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

            if (shop == null)
            {
                throw new Exception();
            }

            var sale = shop.Sales
                .FirstOrDefault(ss => ss.Id == saleId);

            if (sale == null)
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
                var sale = shop.Sales.FirstOrDefault(ss => ss.Id == saleId);

                if (sale == null)
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
                .Include(s => s.Sales)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == shopId);

            if (shop == null)
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