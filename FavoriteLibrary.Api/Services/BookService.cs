using FavoriteLibrary.Dtos;
using FavoriteLibrary.Mappers;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FavoriteLibrary.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BookSearchResponseDto>> GetAllBooksAsync (string? title, string? author)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author))
                    throw new ArgumentException("Debe proporcionar title, author o ambos.");

                var queryParams = new List<string>();

                if (!string.IsNullOrWhiteSpace(title))
                    queryParams.Add($"title={Uri.EscapeDataString(title)}");

                if (!string.IsNullOrWhiteSpace(author))
                    queryParams.Add($"author={Uri.EscapeDataString(author)}");

                queryParams.Add("fields=key,title,author_name,first_publish_year,cover_i");

                var url = $"search.json?{string.Join("&", queryParams)}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var openLibraryResponse = JsonSerializer.Deserialize<OpenLibrarySearchBooksResponse>(json);

                return openLibraryResponse?.Docs.Select(OpenLibraryBookMapper.ToBookSearchDto)
                    .ToList() ?? new List<BookSearchResponseDto>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
