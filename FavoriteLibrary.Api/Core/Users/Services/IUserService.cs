using FavoriteLibrary.Core.Users.Dtos;

namespace FavoriteLibrary.Core.Users.Services
{
    public interface IUserService
    {
        Task<CreateUserResponseDto> AddUserAsync(CreateUserDto dto);
        Task<CreateUserResponseDto?> GetUserByIdAsync(Guid id);

        Task<CreateUserResponseDto> LoginAsync(LoginRequestDto dto);
    }
}
