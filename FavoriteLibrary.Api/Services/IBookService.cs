using FavoriteLibrary.Dtos;

namespace FavoriteLibrary.Services
{
    public interface IBookService
    {
        Task<PagedResultDto<BookSearchResponseDto>> GetAllBooksAsync(
            string? title,
            string? author,
            int page = 1,
            int pageSize = 10);
        
        }
}
