using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.CodeFirst.Models;

public class Author
{
    [Key]
    public int AuthorId { get; set; }

    [Required, StringLength(150)]
    public string Name { get; set; } = null!;

    public string? Bio { get; set; }

    // Navigation
    public ICollection<Book>? Books { get; set; }
}
