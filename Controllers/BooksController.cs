using Library.CodeFirst.Data;
using Library.CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.CodeFirst.Controllers;

public class BooksController : Controller
{
    private readonly ApplicationDbContext _db;
    public BooksController(ApplicationDbContext db) => _db = db;

    // GET: /Books
    public async Task<IActionResult> Index()
    {
        var books = await _db.Books.Include(b => b.Author).Include(b => b.BookGenres!).ThenInclude(bg => bg.Genre).ToListAsync();
        return View(books);
    }

    // GET: /Books/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var book = await _db.Books.Include(b => b.Author).Include(b => b.BookGenres!).ThenInclude(bg => bg.Genre)
            .FirstOrDefaultAsync(b => b.BookId == id);
        if (book == null) return NotFound();
        return View(book);
    }

    // GET: /Books/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Authors = await _db.Authors.ToListAsync();
        ViewBag.Genres = await _db.Genres.ToListAsync();
        return View();
    }

    // POST: /Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book, int[] selectedGenres)
    {
        if (!ModelState.IsValid) return View(book);

        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        if (selectedGenres != null && selectedGenres.Length > 0)
        {
            foreach (var gid in selectedGenres)
            {
                _db.BookGenres.Add(new BookGenre{ BookId = book.BookId, GenreId = gid });
            }
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: /Books/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _db.Books.Include(b => b.BookGenres!).FirstOrDefaultAsync(b => b.BookId == id);
        if (book == null) return NotFound();
        ViewBag.Authors = await _db.Authors.ToListAsync();
        ViewBag.Genres = await _db.Genres.ToListAsync();
        ViewBag.SelectedGenres = book.BookGenres?.Select(bg => bg.GenreId).ToArray() ?? Array.Empty<int>();
        return View(book);
    }

    // POST: /Books/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Book book, int[] selectedGenres)
    {
        if (id != book.BookId) return BadRequest();
        if (!ModelState.IsValid) return View(book);

        _db.Update(book);
        // update many-to-many
        var existing = _db.BookGenres.Where(bg => bg.BookId == id);
        _db.BookGenres.RemoveRange(existing);
        if (selectedGenres != null)
        {
            foreach(var gid in selectedGenres) _db.BookGenres.Add(new BookGenre{ BookId = id, GenreId = gid });
        }
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: /Books/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _db.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.BookId == id);
        if (book == null) return NotFound();
        return View(book);
    }

    // POST: /Books/Delete/5
    [HttpPost, ActionName("Delete") ]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _db.Books.FindAsync(id);
        if (book == null) return NotFound();
        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
