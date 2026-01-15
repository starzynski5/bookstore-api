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
    }
}
