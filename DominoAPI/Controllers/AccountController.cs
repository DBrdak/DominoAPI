using System.Security.Claims;
using DominoAPI.Models.AccountModels;
using DominoAPI.Models.Query;
using DominoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DominoAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
        {
            await _accountService.RegisterUser(dto);

            return Created("You have been registered", null);
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto dto)
        {
            var token = await _accountService.LoginUser(dto);

            return Ok(token);
        }

        [HttpGet("users")]
        [Authorize(Roles = "Head user,Admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserQueryParams query)
        {
            var dto = await _accountService.GetAllUsers(query);

            return Ok(dto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAccount([FromBody] UpdateUserDto dto)
        {
            await _accountService.UpdateUserAccount(dto);

            return Ok();
        }

        [HttpPut("users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUser([FromBody] UpdateExternalUserDto dto, [FromRoute] int userId)
        {
            await _accountService.ManageUser(dto, userId);

            return Ok();
        }

        [HttpDelete("users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId)
        {
            await _accountService.DeleteUser(userId);

            return NoContent();
        }
    }
}