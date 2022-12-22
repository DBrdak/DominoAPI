using DominoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DominoAPI.Controllers
{
    [ApiController]
    [Route("butchery")]
    public class ButcheryController : ControllerBase
    {
        private IButcheryService _butcheryService;

        public ButcheryController(IButcheryService butcheryService)
        {
            _butcheryService = butcheryService;
        }

        [HttpGet("sausages")]
        public async Task<IActionResult> GetAllSausages()
        {
            var sausages = await _butcheryService.GetAllSausages();

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
    }
}