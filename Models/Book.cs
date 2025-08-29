using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.CodeFirst.Models;

public class Book
{
    [Key]
    public int BookId { get; set; }

    [Required, StringLength(250)]
    public string Title { get; set; } = null!;

    [ForeignKey("Author")]
    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    public ICollection<BookGenre>? BookGenres { get; set; }
}
