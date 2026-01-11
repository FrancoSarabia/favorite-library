using FavoriteLibrary.Core.Common.Interfaces;
using FavoriteLibrary.Core.Common.Models;
using FavoriteLibrary.Core.Favorites.Models;
using FavoriteLibrary.Core.Users.Models;
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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Buscamos todas las entidades que hereden de IAuditable 
        // y que estén siendo insertadas o modificadas
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IAuditable && (
                e.State == EntityState.Added ||
                e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var now = DateTime.UtcNow; // Usar siempre UTC es buena práctica

            // Si es una inserción (Add), seteamos CreatedAt
            if (entityEntry.State == EntityState.Added)
            {
                ((IAuditable)entityEntry.Entity).CreatedAt = now;
            }

            // Siempre seteamos el UpdatedAt en inserción o edición
            ((IAuditable)entityEntry.Entity).UpdatedAt = now;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
