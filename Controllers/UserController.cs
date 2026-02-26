using BookStoreAPI.DTOs;
using BookStoreAPI.Models;
using BookStoreAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] User userData)
        {
            if (userData == null) return BadRequest();

            ServiceResponse<User> response = await userService.CreateUser(userData);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = await userService.GetAllUsers();

            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id == 0) return NotFound("User with this id was not found.");

            ServiceResponse<User> response = await userService.GetUserById(id);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUserData)
        {
            if (id == 0 || updatedUserData == null) return BadRequest();

            ServiceResponse<User> response = await userService.UpdateUser(id, updatedUserData);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id == 0) return BadRequest();

            ServiceResponse<User> response = await userService.DeleteUser(id);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpPost]
        [Route("subscribe/{id}")]
        [Authorize]
        
        public async Task<IActionResult> SubscribeToBook(int id)
        {
            int userId = Int32.Parse(User.Identity.Name);

            ServiceResponse<string> response = await userService.SubscribeToBook(userId, id);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }
    }
}
