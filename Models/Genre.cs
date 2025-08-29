using System.ComponentModel.DataAnnotations;

namespace Library.CodeFirst.Models;

public class Genre
{
    [Key]
    public int GenreId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = null!;

    public ICollection<BookGenre>? BookGenres { get; set; }
}
