using FavoriteLibrary.Controllers;
using FavoriteLibrary.Dtos;
using FavoriteLibrary.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FavoriteLibrary.Tests.Controllers
{
    public class FavoriteControllerTests
    {
        private readonly Mock<IFavoriteService> _serviceMock;
        private readonly FavoriteController _controller;

        public FavoriteControllerTests()
        {
            _serviceMock = new Mock<IFavoriteService>();
            _controller = new FavoriteController(_serviceMock.Object);
        }

        [Fact]
        public async Task AddFavorite_ShouldReturnBadRequest_WhenRequestIsInvalid()
        {
            // Arrange
            var serviceMock = new Mock<IFavoriteService>();
            var controller = new FavoriteController(serviceMock.Object);

            var invalidDto = new AddFavoriteBookDto
            {
                ExternalId = "",   // inválido
                Title = "",        // inválido
                Authors = null     // inválido
            };

            // Act
            var result = await controller.AddFavorite(invalidDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteFavorite_ShouldReturnNotFound_WhenFavoriteDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.DeleteFavoriteAsync(id))
                .ThrowsAsync(new KeyNotFoundException("El libro favorito no existe."));

            // Act
            var result = await _controller.DeleteFavorite(id);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult!.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteFavorite_ShouldReturnNoContent_WhenFavoriteExists()
        {
            // Arrange
            var id = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.DeleteFavoriteAsync(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteFavorite(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
