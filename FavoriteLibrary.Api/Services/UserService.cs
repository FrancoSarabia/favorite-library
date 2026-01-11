using FavoriteLibrary.Dtos;
using FavoriteLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FavoriteLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context) {
            _context = context;
        }

        public async Task<CreateUserResponseDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return null;

            return new CreateUserResponseDto
            {
                Id = user.Id,
                firstName = user.firstName,
                lastName = user.lastName,
                userName = user.userName
            };
        }

        public async Task<CreateUserResponseDto> AddUserAsync(CreateUserDto dto)
        {
            bool userExists = await _context.Users
                .AnyAsync(u => u.userName == dto.UserName);

            if (userExists)
                throw new InvalidOperationException("El nombre de usuario ya existe");

            var user = new User
            {
                firstName = dto.FirstName,
                lastName = dto.LastName,
                userName = dto.UserName,
                password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new CreateUserResponseDto
            {
                Id = user.Id,
                firstName = user.firstName,
                lastName = user.lastName,
                userName = user.userName
            };
        }

        public async Task<CreateUserResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.userName == dto.UserName);

            if (user == null)
                throw new InvalidOperationException("Credenciales incorrectas");

            bool passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.password);

            if (!passwordValid)
                throw new InvalidOperationException("Credenciales incorrectas");

            return new CreateUserResponseDto
            {
                Id = user.Id,
                firstName = user.firstName,
                lastName = user.lastName,
                userName = user.userName
            };
        }

    }
}
