using BookStoreAPI.DTOs;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        public IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            ServiceResponse<List<Book>> response = await bookService.GetAllBooks();

            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            if (id == 0) return BadRequest();

            ServiceResponse<Book> response = await bookService.GetBookById(id);

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDTO book)
        {
            if (book == null) return BadRequest();

            ServiceResponse<BookResponseDto> response = await bookService.CreateBook(book);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromBody] CreateBookDTO updatedBook, int id)
        {
            if (updatedBook == null) return BadRequest();

            ServiceResponse<Book> response = await bookService.UpdateBook(updatedBook, id);

            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (id == 0) return BadRequest();

            ServiceResponse<Book> response = await bookService.DeleteBook(id);

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Message);
        }
    }
}
