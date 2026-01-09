using FavoriteLibrary.Dtos;

namespace FavoriteLibrary.Services
{
    public interface IBookService
    {
        Task<List<BookSearchResponseDto>> GetAllBooksAsync(string? title, string? author);
    }
}
