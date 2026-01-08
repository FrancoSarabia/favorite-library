namespace FavoriteLibrary.Dtos
{
    public class FavoriteBookResponseDto
    {
        public Guid Id { get; set; }
        public string ExternalId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int? FirstPublishYear { get; set; }
        public string? CoverUrl { get; set; }
        public List<string> Authors { get; set; } = new();
    }
}
