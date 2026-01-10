using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FavoriteLibrary.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(150)]
        public string firstName { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string lastName { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string userName { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string password { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
