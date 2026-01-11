using FavoriteLibrary.Core.Books.Dtos;
using FavoriteLibrary.Core.Books.Mappers;
using FavoriteLibrary.Core.Common.Dtos;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FavoriteLibrary.Core.Books.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedResultDto<BookSearchResponseDto>> GetAllBooksAsync(
            string? title,
            string? author,
            int page = 1,
            int pageSize = 10
        )
        {
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Debe proporcionar title, author o ambos.");

            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(title))
                queryParams.Add($"title={Uri.EscapeDataString(title)}");

            if (!string.IsNullOrWhiteSpace(author))
                queryParams.Add($"author={Uri.EscapeDataString(author)}");

            queryParams.Add("fields=key,title,author_name,first_publish_year,cover_i");
            queryParams.Add($"page={page}");
            queryParams.Add($"limit={pageSize}");

            var url = $"search.json?{string.Join("&", queryParams)}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var openLibraryResponse =
                JsonSerializer.Deserialize<OpenLibrarySearchBooksResponse>(json);

            return new PagedResultDto<BookSearchResponseDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = openLibraryResponse?.NumFound ?? 0,
                Items = openLibraryResponse?.Docs
                    .Select(OpenLibraryBookMapper.ToBookSearchDto)
                    .ToList() ?? new()
            };
        }

    }
}
