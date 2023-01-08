using System.Text.Json;
using DominoAPI.Models;
using DominoAPI.Models.Create;
using DominoAPI.Models.Update.Butchery;
using DominoAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Newtonsoft.Json;

namespace DominoAPI.Controllers
{
    [ApiController]
    [Route("api/butchery")]
    public class ButcheryController : ControllerBase
    {
        private IButcheryService _butcheryService;

        public ButcheryController(IButcheryService butcheryService)
        {
            _butcheryService = butcheryService;
        }

        [HttpGet("sausages")]
        public async Task<IActionResult> GetAllSausages([FromQuery] string? sausageNameQuery)
        {
            var sausages = await _butcheryService.GetAllSausages(sausageNameQuery);

            return Ok(sausages);
        }

        [HttpGet("sausages/{sausageId}")]
        public async Task<IActionResult> GetSausage([FromRoute] int sausageId)
        {
            var sausage = await _butcheryService.GetSausage(sausageId);

            return Ok(sausage);
        }

        [HttpGet("ingredients/{sausageId}")]
        public async Task<IActionResult> GetAllIngredients([FromRoute] int sausageId)
        {
            var ingredients = await _butcheryService.GetIngredients(sausageId);

            return Ok(ingredients);
        }

        [HttpPost("sausages")]
        public async Task<IActionResult> AddSausage([FromBody] CreateSausageDto dto)
        {
            await _butcheryService.AddSausage(dto);

            return Created("New sausage has been added", null);
        }

        [HttpDelete("sausages/{sausageId}")]
        public async Task<IActionResult> DeleteSausage([FromRoute] int sausageId)
        {
            await _butcheryService.DeleteSausage(sausageId);

            return NoContent();
        }

        [HttpPut("sausages/{sausageId}")]
        public async Task<IActionResult> UpdateSausage([FromRoute] int sausageId, [FromBody] UpdateSausageDto dto)
        {
            await _butcheryService.UpdateSausage(sausageId, dto);

            return Ok();
        }
    }
}