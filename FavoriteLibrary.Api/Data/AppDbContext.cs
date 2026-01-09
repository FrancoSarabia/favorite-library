using FavoriteLibrary.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
        .HasMany(b => b.Authors)
        .WithMany(a => a.Books)
        .UsingEntity<Dictionary<string, object>>(
        "BookAuthors",
        j => j
            .HasOne<Author>()
            .WithMany()
            .HasForeignKey("AuthorId"),
        j => j
            .HasOne<Book>()
            .WithMany()
            .HasForeignKey("BookId")
        );
    }
}
