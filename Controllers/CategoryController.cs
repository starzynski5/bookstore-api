using BookStoreAPI.DTOs;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        public ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO category)
        {
            if (category == null)
            {
                return BadRequest("Category data is null.");
            }

            ServiceResponse<Category> response = await categoryService.CreateCategory(category);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            ServiceResponse<List<CategoryResponseDTO>> response = await categoryService.GetAllCategories();

            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            if (id == 0) return BadRequest();

            ServiceResponse<Category> response = await categoryService.GetCategoryById(id);

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory([FromBody] Category updatedCategory, int id)
        {
            if (id == 0) return BadRequest();

            ServiceResponse<Category> response = await categoryService.UpdateCategory(updatedCategory, id);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id == 0) return BadRequest();

            ServiceResponse<Category> response = await categoryService.DeleteCategory(id);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }
    }
}
