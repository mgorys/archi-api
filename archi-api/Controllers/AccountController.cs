using Microsoft.AspNetCore.Mvc;
using Archi.Models;
using Archi.Services.Account;

namespace Archi.Controllers
{

    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
           _accountService = accountService;
        }
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUserAsync([FromBody] RegisterUserDto dto)
        {
            await _accountService.RegisterUserAsync(dto);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            string token = await _accountService.GenerateJwtAsync(dto);
            return Ok(token);
        }
    }

}
