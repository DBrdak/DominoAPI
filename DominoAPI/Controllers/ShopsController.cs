using DominoAPI.Models.Create.Shops;
using DominoAPI.Models.Query;
using DominoAPI.Models.Update.Shops;
using DominoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DominoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/shops")]
    public class ShopsController : ControllerBase
    {
        private readonly IShopsService _shopsService;

        public ShopsController(IShopsService shopsService)
        {
            _shopsService = shopsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShops()
        {
            var dto = await _shopsService.GetAllShops();

            return Ok(dto);
        }

        [HttpGet("{shopId}")]
        public async Task<IActionResult> GetShopDetails([FromRoute] int shopId)
        {
            var shop = await _shopsService.GetShopDetails(shopId);

            return Ok(shop);
        }

        [HttpGet("{shopId}/sales")]
        [Authorize(Roles = "Admin,Head user,Employee")]
        public async Task<IActionResult> GetSales([FromRoute] int shopId, [FromQuery] SalesQueryParams query)
        {
            var shop = await _shopsService.GetSales(shopId, query);

            return Ok(shop);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Head user,Employee")]
        public async Task<IActionResult> AddMobileShop([FromBody] CreateShopDto dto)
        {
            await _shopsService.AddShop(dto);

            return Created("New shop has been created", null);
        }

        [HttpPost("{shopId}/sales")]
        [Authorize(Roles = "Admin,Head user,Employee")]
        public async Task<IActionResult> AddNewSale([FromRoute] int shopId, [FromBody] CreateSaleDto dto)
        {
            await _shopsService.AddNewSale(dto, shopId);

            return Created("New sale has been added", null);
        }

        [HttpPost("sales/{shopId}")]
        [Authorize(Roles = "Admin,Head user,Employee")]
        public async Task<IActionResult> AddNewRecentSale([FromRoute] int shopId, [FromBody] CreateSaleDto dto)
        {
            await _shopsService.AddNewRecentSale(dto, shopId);

            return Created("New sale has been added", null);
        }

        [HttpPut("{shopId}")]
        [Authorize(Roles = "Admin,Head user,Employee")]
        public async Task<IActionResult> UpdateShop([FromRoute] int shopId, [FromBody] UpdateShopDto dto)
        {
            await _shopsService.UpdateShop(dto, shopId);

            return Ok();
        }

        [HttpDelete("{shopId}")]
        [Authorize(Roles = "Admin,Head user")]
        public async Task<IActionResult> DeleteShop([FromRoute] int shopId)
        {
            await _shopsService.DeleteShop(shopId);

            return NoContent();
        }

        [HttpDelete("{shopId}/sales/{saleId}")]
        [Authorize(Roles = "Admin,Head user")]
        public async Task<IActionResult> DeleteSale([FromRoute] int shopId, [FromRoute] int saleId)
        {
            await _shopsService.DeleteSale(shopId, saleId);

            return NoContent();
        }

        [HttpDelete("{shopId}/sales")]
        [Authorize(Roles = "Admin,Head user")]
        public async Task<IActionResult> DeleteSalesRange([FromRoute] int shopId, [FromBody] List<int> salesId)
        {
            await _shopsService.DeleteSalesRange(shopId, salesId);

            return NoContent();
        }

        [HttpDelete("{shopId}/sales/all")]
        [Authorize(Roles = "Admin,Head user")]
        public async Task<IActionResult> DeleteAllSales([FromRoute] int shopId)
        {
            await _shopsService.DeleteAllSales(shopId);

            return NoContent();
        }
    }
}