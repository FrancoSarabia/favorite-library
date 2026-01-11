using System.ComponentModel.DataAnnotations;

namespace FavoriteLibrary.Core.Favorites.Dtos
{
    public class AddFavoriteBookDto
    {
        [Required]
        public string ExternalId { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;

        public int? FirstPublishYear { get; set; }

        [Required]
        [MinLength(1)]
        public List<string> Authors { get; set; } = new();

        public string? CoverUrl { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
