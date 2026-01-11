using FavoriteLibrary.Core.Common.Interfaces;
using FavoriteLibrary.Core.Favorites.Models;
using System.ComponentModel.DataAnnotations;

namespace FavoriteLibrary.Core.Common.Models
{
    public class Author: IAuditable
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = new List<Book>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
