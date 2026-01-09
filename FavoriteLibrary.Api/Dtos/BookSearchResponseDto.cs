namespace FavoriteLibrary.Dtos
{
    public class BookSearchResponseDto
    {
        public string Title { get; set; } = null!;
        public int? FirstPublishYear { get; set; }
        public List<string> Authors { get; set; } = new();
        public string? CoverUrl { get; set; }

        public string ExternalId { get; set; } = null!;
    }
}
