using FavoriteLibrary.Dtos;
using FavoriteLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace FavoriteLibrary.Services
{
    public interface IFavoriteService
    {
        Task<List<FavoriteBookResponseDto>> GetFavoritesAsync();
        Task<List<FavoriteBookResponseDto>> GetFavoritesByUserAsync(Guid userId);
        Task<FavoriteBookResponseDto> AddFavoriteAsync(AddFavoriteBookDto dto);
        Task DeleteFavoriteAsync(Guid id);
    }
}
