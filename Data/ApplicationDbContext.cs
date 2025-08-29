using Library.CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.CodeFirst.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<BookGenre> BookGenres => Set<BookGenre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookGenre>()
            .HasKey(bg => new { bg.BookId, bg.GenreId });

        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Book)
            .WithMany(b => b.BookGenres)
            .HasForeignKey(bg => bg.BookId);

        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Genre)
            .WithMany(g => g.BookGenres)
            .HasForeignKey(bg => bg.GenreId);

        // Seed sample data (optional)
        modelBuilder.Entity<Author>().HasData(
            new Author{ AuthorId = 1, Name = "George Orwell", Bio = "English novelist"},
            new Author{ AuthorId = 2, Name = "Jane Austen", Bio = "English novelist"}
        );

        modelBuilder.Entity<Genre>().HasData(
            new Genre{ GenreId = 1, Name = "Dystopian"},
            new Genre{ GenreId = 2, Name = "Romance"}
        );

        modelBuilder.Entity<Book>().HasData(
            new Book{ BookId = 1, Title = "1984", AuthorId = 1},
            new Book{ BookId = 2, Title = "Pride and Prejudice", AuthorId = 2}
        );

        modelBuilder.Entity<BookGenre>().HasData(
            new BookGenre{ BookId = 1, GenreId = 1},
            new BookGenre{ BookId = 2, GenreId = 2}
        );
    }
}
