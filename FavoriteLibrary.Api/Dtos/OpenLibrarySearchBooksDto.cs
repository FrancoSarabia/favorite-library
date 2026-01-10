using System.Text.Json.Serialization;

namespace FavoriteLibrary.Dtos
{
    public class OpenLibrarySearchBooksResponse
    {
        [JsonPropertyName("docs")]
        public List<OpenLibraryBookDto> Docs { get; set; } = new();

        [JsonPropertyName("numFound")]
        public int NumFound { get; set; }
    }
    public class OpenLibraryBookDto
    {
        [JsonPropertyName("key")]
        public string? ExternalIdBook { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("author_name")]
        public List<string>? Authors { get; set; }
        
        [JsonPropertyName("first_publish_year")]
        public int? FirstPublishYear { get; set; }

        [JsonPropertyName("cover_i")]
        public int? CoverId { get; set; }

    }
}
