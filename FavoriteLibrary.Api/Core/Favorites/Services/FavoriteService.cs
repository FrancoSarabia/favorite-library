using FavoriteLibrary.Core.Common.Dtos;
using FavoriteLibrary.Core.Common.Models;
using FavoriteLibrary.Core.Favorites.Dtos;
using FavoriteLibrary.Core.Favorites.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FavoriteLibrary.Core.Favorites.Services
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

        public async Task<List<FavoriteBookResponseDto>> GetFavoritesByUserAsync(Guid userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);

            if (!userExists)
                throw new InvalidOperationException("Usuario no existe.");

            var books = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Users)
                .Where(b => b.Users.Any(u => u.Id == userId))
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
                Authors = b.Authors.Select(a => a.Name).ToList(),
                User = userId
            }).ToList();
        }

        public async Task<PagedResultDto<FavoriteBookResponseDto>> GetFavoritesByUserPaginatedAsync(Guid userId, int page,int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new InvalidOperationException("Usuario no existe.");

            var query = _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Users)
                .Where(b => b.Users.Any(u => u.Id == userId));

            var totalItems = await query.CountAsync();

            var books = await query
                .OrderBy(b => b.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDto<FavoriteBookResponseDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = totalItems,
                Items = books.Select(b => new FavoriteBookResponseDto
                {
                    Id = b.Id,
                    ExternalId = b.BookExternalId,
                    Title = b.Title,
                    FirstPublishYear = b.FirstPublishYear == DateTime.MinValue
                        ? null
                        : b.FirstPublishYear.Year,
                    CoverUrl = b.CoverUrl,
                    Authors = b.Authors.Select(a => a.Name).ToList(),
                    User = userId
                }).ToList()
            };
        }


        public async Task<FavoriteBookResponseDto> AddFavoriteAsync(AddFavoriteBookDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Books)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user == null)
                throw new InvalidOperationException("Usuario no existe.");

            bool alreadyFavorite = await _context.Books
                .AnyAsync(b =>
                    b.BookExternalId == dto.ExternalId &&
                    b.Users.Any(u => u.Id == dto.UserId));

            if (alreadyFavorite)
                throw new InvalidOperationException("El libro ya está en favoritos del usuario.");

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

            var book = await _context.Books
                .Include(b => b.Users)
                .FirstOrDefaultAsync(b => b.BookExternalId == dto.ExternalId);

            if (book == null)
            {
                book = new Book
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
            }
            book.Users.Add(user);

            await _context.SaveChangesAsync();
            return formatedCreateFavoriteResponse(book, dto.UserId);
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

        private static FavoriteBookResponseDto formatedCreateFavoriteResponse( Book book, Guid userId)
        {
            return new FavoriteBookResponseDto
            {
                Id = book.Id,
                ExternalId = book.BookExternalId,
                Title = book.Title,
                FirstPublishYear = book.FirstPublishYear.Year,
                CoverUrl = book.CoverUrl,
                Authors = book.Authors.Select(a => a.Name).ToList(),
                User = userId
            };
        }
    }
}
