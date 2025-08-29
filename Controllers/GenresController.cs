using Library.CodeFirst.Data;
using Library.CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.CodeFirst.Controllers;

public class GenresController : Controller
{
    private readonly ApplicationDbContext _db;
    public GenresController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Genres.ToListAsync());
    public IActionResult Create() => View();
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Genre genre)
    {
        if (!ModelState.IsValid) return View(genre);
        _db.Genres.Add(genre);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var g = await _db.Genres.FindAsync(id);
        if (g==null) return NotFound();
        return View(g);
    }
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Genre genre)
    {
        if (id != genre.GenreId) return BadRequest();
        if (!ModelState.IsValid) return View(genre);
        _db.Update(genre);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var g = await _db.Genres.FindAsync(id);
        if (g==null) return NotFound();
        return View(g);
    }
    [HttpPost, ActionName("Delete")][ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var g = await _db.Genres.FindAsync(id);
        if (g==null) return NotFound();
        _db.Genres.Remove(g);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
