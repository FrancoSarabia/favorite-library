using FavoriteLibrary.Models;
using FavoriteLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FavoriteLibrary.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {

        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? title,
            [FromQuery] string? author)
        {
            var result = await _service.GetAllBooksAsync(title, author);
            return Ok(result);
        }
    }
}
