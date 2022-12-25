using DominoAPI.Entities;
using DominoAPI.Models;
using DominoAPI.Models.Create;
using DominoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DominoAPI.Controllers
{
    [ApiController]
    [Route("priceList")]
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

        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto dto)
        {
            await _priceListService.AddProduct(dto);

            return Created("New product has been added to price list", null);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto dto, [FromRoute] int id)
        {
            await _priceListService.UpdateProduct(dto, id);

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            await _priceListService.DeleteProduct(id);

            return NoContent();
        }
    }
}