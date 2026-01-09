using FavoriteLibrary.Dtos;
using FavoriteLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FavoriteLibrary.Services
{
    public class FavoriteService : IFavoriteService
    {

        private readonly AppDbContext _context;

        public FavoriteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FavoriteBookResponseDto>> GetFavoritesAsync()
        {
            var books = await _context.Books
                .Include(b => b.Authors)
                .OrderBy(b => b.Title)
                .ToListAsync();

            return books.Select(b => new FavoriteBookResponseDto
            {
                Id = b.Id,
                ExternalId = b.BookExternalId,
                Title = b.Title,
                FirstPublishYear = b.FirstPublishYear == DateTime.MinValue
                    ? null
                    : b.FirstPublishYear.Year,
                CoverUrl = b.CoverUrl,
                Authors = b.Authors.Select(a => a.Name).ToList()
            }).ToList();
        }

        public async Task<FavoriteBookResponseDto> AddFavoriteAsync(AddFavoriteBookDto dto)
        {

            var existingBook = await _context.Books.AnyAsync(b => b.BookExternalId == dto.ExternalId);

            if (existingBook)
                throw new InvalidOperationException("El libro ya está agregado como favorito.");

            var authors = new List<Author>();

            foreach (var authorName in dto.Authors)
            {
                var author = await _context.Authors
                    .FirstOrDefaultAsync(a => a.Name == authorName);

                if (author == null)
                {
                    author = new Author { Name = authorName };
                    _context.Authors.Add(author);
                }

                authors.Add(author);
            }

            var book = new Book
            {
                Title = dto.Title,
                FirstPublishYear = dto.FirstPublishYear.HasValue
                    ? new DateTime(dto.FirstPublishYear.Value, 1, 1)
                    : DateTime.MinValue,
                CoverUrl = dto.CoverUrl,
                BookExternalId = dto.ExternalId,
                Authors = authors
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            FavoriteBookResponseDto response = formatedCreateFavoriteResponse(book);

            return response;
        }

        public async Task DeleteFavoriteAsync(Guid id)
        {
            var book = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                throw new KeyNotFoundException("El libro favorito no existe.");

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        private static FavoriteBookResponseDto formatedCreateFavoriteResponse( Book book )
        {
            return new FavoriteBookResponseDto
            {
                Id = book.Id,
                ExternalId = book.BookExternalId,
                Title = book.Title,
                FirstPublishYear = book.FirstPublishYear.Year,
                CoverUrl = book.CoverUrl,
                Authors = book.Authors.Select(a => a.Name).ToList()
            };
        }
    }
}
