using FavoriteLibrary.Core.Favorites.Dtos;
using FavoriteLibrary.Core.Favorites.Services;
using Microsoft.AspNetCore.Mvc;

namespace FavoriteLibrary.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _service;

        public FavoriteController(IFavoriteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var favorites = await _service.GetFavoritesAsync();
            return Ok(favorites);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetFavoritesByUser(Guid userId)
        {
            try
            {
                var favorites = await _service.GetFavoritesByUserAsync(userId);
                return Ok(favorites);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user-paginated/{userId:guid}")]
        public async Task<IActionResult> GetFavoritesByUserPaginated(
            Guid userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
)
        {
            try
            {
                var favorites = await _service.GetFavoritesByUserPaginatedAsync(userId, page, pageSize);
                return Ok(favorites);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteBookDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");

            if (string.IsNullOrWhiteSpace(dto.ExternalId) ||
                string.IsNullOrWhiteSpace(dto.Title) ||
                dto.Authors == null || !dto.Authors.Any())
            {
                return BadRequest("Invalid request data.");
            }
            try
            {
                var response = await _service.AddFavoriteAsync(dto);
                return CreatedAtAction(nameof(AddFavorite), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteFavorite(Guid id)
        {
            try
            {
                await _service.DeleteFavoriteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
