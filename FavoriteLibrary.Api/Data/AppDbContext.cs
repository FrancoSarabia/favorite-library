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
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books)
            .UsingEntity<Dictionary<string, object>>(
                "BookAuthors",
                j => j
                    .HasOne<Author>()
                    .WithMany()
                    .HasForeignKey("AuthorId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Book>()
                    .WithMany()
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("BookId", "AuthorId");
                    j.ToTable("BookAuthors");
                }
            );

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Users)
            .WithMany(u => u.Books)
            .UsingEntity<Dictionary<string, object>>(
                "BookUser",
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Book>()
                    .WithMany()
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("BookId", "UserId");
                    j.ToTable("BookUsers");
                }
            );
    }
}
