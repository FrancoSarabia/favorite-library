using FavoriteLibrary.Core.Books.Dtos;
using FavoriteLibrary.Core.Common.Dtos;

namespace FavoriteLibrary.Core.Books.Services
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
