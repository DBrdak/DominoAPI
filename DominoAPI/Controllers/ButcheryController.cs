﻿using System.Text.Json;
using DominoAPI.Models;
using DominoAPI.Models.Create;
using DominoAPI.Models.Update;
using DominoAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Newtonsoft.Json;

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

        [HttpPost("sausages/add")]
        public async Task<IActionResult> AddSausage([FromBody] CreateSausageDto dto)
        {
            await _butcheryService.AddSausage(dto);

            return Ok();
        }

        [HttpDelete("sausages/delete/{sausageId}")]
        public async Task<IActionResult> DeleteSausage([FromRoute] int sausageId)
        {
            await _butcheryService.DeleteSausage(sausageId);

            return NoContent();
        }

        [HttpPut("sausages/update/{sausageId}")]
        public async Task<IActionResult> UpdateSausage([FromRoute] int sausageId, [FromBody] UpdateSausageDto dto)
        {
            await _butcheryService.UpdateSausage(sausageId, dto);

            return Ok();
        }
    }
}