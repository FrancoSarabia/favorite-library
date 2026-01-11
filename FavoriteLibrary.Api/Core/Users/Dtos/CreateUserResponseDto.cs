namespace FavoriteLibrary.Core.Users.Dtos
{
    public class CreateUserResponseDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string firstName { get; set; } = null!;

        public string lastName { get; set; } = null!;

        public string userName { get; set; } = null!;
    }
}
