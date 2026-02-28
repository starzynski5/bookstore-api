using Azure;
using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services
{
    public class UserService : IUserService
    {
        private readonly BooksStoreDbContext _context;

        public UserService(BooksStoreDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<User>> CreateUser(User userData)
        {
            ServiceResponse<User> response = new ServiceResponse<User>();

            bool isExistingUsername = await _context.Users
                .Where(u => u.Username == userData.Username)
                .AnyAsync();

            bool isExistingEmail = await _context.Users
                .Where(u => u.Email == userData.Email)
                .AnyAsync();

            if (isExistingEmail || isExistingUsername)
            {
                response.Success = false;
                response.Message = "Username or Email already exists.";

                return response;
            }

            User newUser = new User
            {
                Username = userData.Username,
                Email = userData.Email,
                Password = userData.Password
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "User created successfully.";
            response.Data = newUser;

            return response;
        }

        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = await _context.Users.ToListAsync();

            return users;
        }

        public async Task<ServiceResponse<User>> GetUserById(int id)
        {
            ServiceResponse<User> response = new ServiceResponse<User>();

            User user = await _context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                response.Success = false;
                response.Message = "User with this id was not found.";

                return response;
            }

            response.Success = true;
            response.Data = user;

            return response;
        }

        public async Task<ServiceResponse<User>> UpdateUser(int id, User updatedUserData)
        {
            ServiceResponse<User> response = new ServiceResponse<User>();

            User user = await _context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                response.Success = false;
                response.Message = "User with this id was not found.";
                return response;
            }

            if (updatedUserData.Username != user.Username && updatedUserData.Username != null)
            {
                user.Username = updatedUserData.Username;
            }

            if (updatedUserData.Email != user.Email && updatedUserData.Email != null)
            {
                user.Email = updatedUserData.Email;
            }

            if (updatedUserData.Password != user.Password && updatedUserData.Password != null)
            {
                user.Password = updatedUserData.Password;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "User updated successfully.";
            response.Data = user;

            return response;

        }

        public async Task<ServiceResponse<User>> DeleteUser(int id)
        {
            ServiceResponse<User> response = new ServiceResponse<User>();

            User user = await _context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                response.Success = false;
                response.Message = "User with this id was not found.";
                return response;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "User deleted successfully.";
            response.Data = user;

            return response;
        }

        public async Task<ServiceResponse<string>> SubscribeToBook(int userId, int bookId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            User isUserExisting = await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            Book book = await _context.Books
                .Where(b => b.Id == bookId)
                .FirstOrDefaultAsync();

            if (isUserExisting == null || book == null)
            {
                response.Success = false;
                response.Message = "User or Book with this id was not found.";
            }

            User user = await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.UserBooks)
                .FirstOrDefaultAsync();

            if (user.UserBooks.Any(ub => ub.BookId == bookId))
            {
                response.Success = false;
                response.Message = "User is already subscribed to this book.";
                return response;
            }

            UserBook userBook = new UserBook
            {
                UserId = userId,
                BookId = bookId,
                Progress = 0,
                LastTimeRead = DateTime.UtcNow
            };

            _context.UserBooks.Add(userBook);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "User subscribed to book successfully.";

            return response;

        }

        public async Task<ServiceResponse<List<Book>>> GetAllSubscribedBook(int userId)
        {
            ServiceResponse<List<Book>> response = new ServiceResponse<List<Book>>();

            List<Book> subscribedBooks = await _context.UserBooks
                .Where(ub => ub.UserId == userId)
                .Include(ub => ub.Book)
                .Select(ub => ub.Book)
                .ToListAsync();

            response.Success = true;
            response.Data = subscribedBooks;
            return response;
        }
    }
}
