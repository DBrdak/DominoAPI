using DominoAPI.Entities.PriceList;
using DominoAPI.Models.Create.PriceList;
using DominoAPI.Models.Update.PriceList;
using DominoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DominoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/price-list")]
    public class PriceListController : ControllerBase
    {
        private IPriceListService _priceListService;

        public PriceListController(IPriceListService priceListService)
        {
            _priceListService = priceListService;
        }

        [HttpGet("{productType}")]
        public async Task<IActionResult> GetAllProducts([FromRoute] ProductType productType)
        {
            var products = await _priceListService.GetAllProducts(productType);

            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Head user,Employee")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto dto)
        {
            await _priceListService.AddProduct(dto);

            return Created("New product has been added to price list", null);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Head user,Employee")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto dto, [FromRoute] int id)
        {
            await _priceListService.UpdateProduct(dto, id);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Head user")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            await _priceListService.DeleteProduct(id);

            return NoContent();
        }
    }
}