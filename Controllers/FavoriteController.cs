using FavoriteLibrary.Dtos;
using FavoriteLibrary.Services;
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

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteBookDto dto)
        {
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
                return NotFound(ex.Message);
            }
        }
    }
}
