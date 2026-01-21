using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services
{
    public class BookService : IBookService
    {
        public readonly BooksStoreDbContext _context;

        public BookService(BooksStoreDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Book>>> GetAllBooks()
        {
            List<Book> books = await _context.Books
                .ToListAsync();

            ServiceResponse<List<Book>> response = new ServiceResponse<List<Book>>();
            response.Success = true;
            response.Data = books;

            return response;
        }

        public async Task<ServiceResponse<Book>> GetBookById(int id)
        {
            ServiceResponse<Book> response = new ServiceResponse<Book>();

            Book? book = await _context.Books
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                response.Success = false;
                response.Message = "Book with this id was not found.";

                return response;
            }

            response.Success = true;
            response.Data = book;

            return response;
        }
    }
}
