using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.ResponseCaching;
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

        public async Task<ServiceResponse<BookResponseDto>> CreateBook(CreateBookDTO book)
        {
            ServiceResponse<BookResponseDto> response = new ServiceResponse<BookResponseDto>();

            if (book.CategoryId == null || book.Content == null || book.Author == null || book.CoverLink == null)
            {
                response.Success = false;
                response.Message = "Creating book failed. Please try again.";

                return response;
            }

            Category existingCategory = await _context.Categories
                .Where(c => c.Id == book.CategoryId)
                .FirstOrDefaultAsync();

            if (existingCategory == null)
            {
                response.Success = false;
                response.Message = "Creating book failed. Category does not exist.";

                return response;
            }

            Book newBook = new Book
            {
                Title = book.Title,
                Author = book.Author,
                Content = book.Content,
                CoverLink = book.CoverLink,
                CategoryId = book.CategoryId
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            var responseDto = new BookResponseDto
            {
                Id = newBook.Id,
                Title = newBook.Title,
                Author = newBook.Author,
                Content = newBook.Content,
                CoverLink = newBook.CoverLink,
                CategoryId = newBook.CategoryId,
                CategoryName = (await _context.Categories
                    .Where(c => c.Id == newBook.CategoryId)
                    .Select(c => c.Name)
                    .FirstAsync())
            };

            response.Success = true;
            response.Data = responseDto;

            return response;

        }

        public async Task<ServiceResponse<Book>> UpdateBook(CreateBookDTO updatedBook, int id)
        {
            ServiceResponse<Book> response = new ServiceResponse<Book>();

            Book book = await _context.Books
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            Category existingCategory = await _context.Categories
                .Where(c => c.Id == updatedBook.CategoryId)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                response.Success = false;
                response.Message = "Book with this id was not found.";

                return response;
            }

            if (existingCategory == null)
            {
                response.Success = false;
                response.Message = "Category with this id was not found.";

                return response;
            }

            book.Author = updatedBook.Author;
            book.CoverLink = updatedBook.CoverLink;
            book.Content = updatedBook.Content;
            book.Title = updatedBook.Title;
            book.CategoryId = updatedBook.CategoryId;

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = book;
            response.Message = "Book updated successfully.";

            return response;
        }

        public async Task<ServiceResponse<Book>> DeleteBook(int id)
        {
            ServiceResponse<Book> response = new ServiceResponse<Book>();

            Book book = await _context.Books
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                response.Success = false;
                response.Message = "Book with this id was not found.";

                return response;
            }

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Book removed successfully.";

            return response;
        }
    }
}
