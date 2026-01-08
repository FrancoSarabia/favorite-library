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

        //[HttpPost]
        //public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteRequest request)
        //{
        //    if (string.IsNullOrWhiteSpace(request.OpenLibraryId) && string.IsNullOrWhiteSpace(request.Isbn))
        //        return BadRequest("OpenLibraryId o ISBN requerido.");

        //    // Obtener datos desde Open Library
        //    var externalBook = await _service.GetBookDetailsAsync(request.OpenLibraryId, request.Isbn);

        //    if (externalBook == null)
        //        return NotFound("No se encontró el libro en Open Library.");

        //    // Buscar o crear autores
        //    var authorEntities = new List<Author>();
        //    foreach (var authorName in externalBook.AuthorNames)
        //    {
        //        var existingAuthor = await _context.Authors
        //            .FirstOrDefaultAsync(a => a.Name == authorName);

        //        if (existingAuthor != null)
        //            authorEntities.Add(existingAuthor);
        //        else
        //        {
        //            var newAuthor = new Author { Name = authorName };
        //            authorEntities.Add(newAuthor);
        //            _context.Authors.Add(newAuthor);
        //        }
        //    }

        //    // Crear libro en DB
        //    var book = new Book
        //    {
        //        Title = externalBook.Title,
        //        FirstPublishYear = externalBook.FirstPublishYear,
        //        CoverUrl = externalBook.CoverUrl,
        //        Authors = authorEntities
        //    };

        //    _context.Books.Add(book);
        //    await _context.SaveChangesAsync();

        //    return Ok(book);
        //}
    }
}
