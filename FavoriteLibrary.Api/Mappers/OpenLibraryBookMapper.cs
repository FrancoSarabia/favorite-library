using FavoriteLibrary.Dtos;

namespace FavoriteLibrary.Mappers
{
    public static class OpenLibraryBookMapper
    {
        public static BookSearchResponseDto ToBookSearchDto(OpenLibraryBookDto source)
        {
            return new BookSearchResponseDto
            {
                ExternalId = source.ExternalIdBook ?? string.Empty,
                Title = source.Title ?? "Unknown",
                FirstPublishYear = source.FirstPublishYear,
                Authors = source.Authors ?? new List<string>(),
                CoverUrl = source.CoverId != null
                    ? $"https://covers.openlibrary.org/b/id/{source.CoverId}-L.jpg"
                    : null
            };
        }
    }
}
