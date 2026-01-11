using System.ComponentModel.DataAnnotations;

namespace FavoriteLibrary.Core.Users.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [MinLength(3)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string LastName { get; set; } = null!;

        [Required]
        [MinLength(3)]
        public string UserName { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
    }
}
