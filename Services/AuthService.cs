using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStoreAPI.Services
{
    public class AuthService : IAuthService
    {
        public JWTService _jwtService;
        public BooksStoreDbContext _context;

        public AuthService(JWTService jwtService, BooksStoreDbContext context)
        {
            _jwtService = jwtService;
            _context = context;
        }

        public async Task<ServiceResponse<string>> Register(RegisterRequestDTO request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            User isEmailTaken = await _context.Users
                .Where(u => u.Email == request.Email)
                .FirstOrDefaultAsync();

            User isUsernameTaken = await _context.Users
                .Where(u => u.Username == request.Username)
                .FirstOrDefaultAsync();

            if (isEmailTaken != null)
            {
                response.Success = false;
                response.Message = "Email is already taken.";

                return response;
            }

            if (isUsernameTaken != null)
            {
                response.Success = false;
                response.Message = "Username is already taken.";

                return response;
            }

            User newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Registration successful.";
            response.Data = _jwtService.GenerateToken(newUser);

            return response;
        }

        public async Task<ServiceResponse<string>> Login(LoginRequestDTO request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            User user = await _context.Users
                .Where(u => u.Email.ToLower() == request.Email.ToLower()
                         && u.Password == request.Password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid email or password.";

                return response;
            }

            response.Success = true;
            response.Message = "Login successful.";
            response.Data = _jwtService.GenerateToken(user);

            return response;
        }
    }
}
