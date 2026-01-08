using System.ComponentModel.DataAnnotations;

namespace FavoriteLibrary.Models
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;

        public DateTime FirstPublishYear { get; set; }

        public string? CoverUrl { get; set; } = null!;

        public ICollection<Author> Authors { get; set; } = new List<Author>();

        public string BookExternalId { get; set; } = null!;

    }
}
