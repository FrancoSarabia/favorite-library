namespace FavoriteLibrary.Dtos
{
    public class AddFavoriteBookDto
    {
        public string ExternalId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int? FirstPublishYear { get; set; }
        public List<string> Authors { get; set; } = new();
        public string? CoverUrl { get; set; }
    }
}
