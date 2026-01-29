using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        public IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO request)
        {
            ServiceResponse<string> response = await _service.Register(request);

            if (response.Success == false)
            {
                return Conflict(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            ServiceResponse<string> response = await _service.Login(request);

            if (response.Success == false)
            {
                return Conflict(response.Message);
            }

            return Ok(response.Data);
        }
    }
}
