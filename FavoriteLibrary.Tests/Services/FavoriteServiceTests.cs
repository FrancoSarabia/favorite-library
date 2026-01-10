using FavoriteLibrary.Dtos;
using FavoriteLibrary.Models;
using FavoriteLibrary.Services;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace FavoriteLibrary.Tests;

public class FavoriteServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddFavoriteAsync_ShouldThrowException_WhenBookAlreadyExists()
    {
        // Arrange
        var context = CreateDbContext();
        var service = new FavoriteService(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            firstName = "Test",
            lastName = "User",
            userName = "testuser",
            password = "hashed"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var dto = new AddFavoriteBookDto
        {
            ExternalId = "/works/OL45804W",
            Title = "Fantastic Mr Fox",
            FirstPublishYear = 1928,
            CoverUrl = "https://covers.openlibrary.org/b/id/15152634-S.jpg",
            Authors = new List<string> { "Virginia Woolf" },
            UserId = user.Id
        };

        // Primera inserción OK
        await service.AddFavoriteAsync(dto);

        // Act
        Func<Task> act = async () => await service.AddFavoriteAsync(dto);

        // Assert
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("El libro ya está en favoritos del usuario.");
    }

}
