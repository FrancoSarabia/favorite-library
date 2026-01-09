    using FavoriteLibrary.Dtos;
using FavoriteLibrary.Mappers;
using FluentAssertions;
using Xunit;


namespace FavoriteLibrary.Tests.Mappers
{
    public class OpenLibraryBookMapperTests
    {
        [Fact]
        public void ToBookSearchDto_ShouldNormalizeExternalApiData()
        {
            // Arrange
            var externalDto = new OpenLibraryBookDto
            {
                ExternalIdBook = "/works/OL123W",
                Title = "Clean Code",
                FirstPublishYear = 2008,
                Authors = new List<string> { "Robert C. Martin" },
                CoverId = 12345
            };

            // Act
            var result = OpenLibraryBookMapper.ToBookSearchDto(externalDto);

            // Assert
            result.ExternalId.Should().Be("/works/OL123W");
            result.Title.Should().Be("Clean Code");
            result.FirstPublishYear.Should().Be(2008);
            result.Authors.Should().ContainSingle("Robert C. Martin");
            result.CoverUrl.Should().Be("https://covers.openlibrary.org/b/id/12345-L.jpg");
        }

        [Fact]
        public void ToBookSearchDto_ShouldHandleNullValuesGracefully()
        {
            // Arrange
            var externalDto = new OpenLibraryBookDto
            {
                ExternalIdBook = null,
                Title = null,
                Authors = null,
                CoverId = null
            };

            // Act
            var result = OpenLibraryBookMapper.ToBookSearchDto(externalDto);

            // Assert
            result.ExternalId.Should().BeEmpty();
            result.Title.Should().Be("Unknown");
            result.Authors.Should().BeEmpty();
            result.CoverUrl.Should().BeNull();
        }

    }
}
