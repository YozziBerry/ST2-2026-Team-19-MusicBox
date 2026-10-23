
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicBox.Models;
using MusicBox.Data;
using MusicBox.Services;

public class SongsController : Controller
{
    private readonly MusicBoxDbContext _context;

    public SongsController(MusicBoxDbContext context)
    {
        _context = context;
    }

    // GET: SONGS
    public async Task<IActionResult> Index(string? searchString)
    {
        IQueryable<Song> songs = _context.Songs;

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            songs = songs.Where(s =>
                s.Title.Contains(searchString) ||
                s.Artist.Contains(searchString) ||
                (s.Genre != null &&
                 s.Genre.Contains(searchString)));
        }

        ViewData["CurrentFilter"] = searchString;
        ViewData["VisitCount"] =
            MusicBoxVisitCounter.Instance.RegisterVisit();

        return View(await songs.OrderBy(s => s.Title).ToListAsync());
    }

    // GET: SONGS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var song = await _context.Songs
            .FirstOrDefaultAsync(m => m.Id == id);
        if (song == null)
        {
            return NotFound();
        }

        return View(song);
    }

    // GET: Songs/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Songs/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Title,Artist,Genre,ReleaseYear")] Song song)
    {
        if (!ModelState.IsValid)
        {
            return View(song);
        }

        _context.Songs.Add(song);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: SONGS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var song = await _context.Songs.FindAsync(id);
        if (song == null)
        {
            return NotFound();
        }
        return View(song);
    }

    // POST: SONGS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Artist,Genre,ReleaseYear")] Song song)
    {
        if (id != song.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(song);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(song.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(song);
    }

    // GET: SONGS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var song = await _context.Songs
            .FirstOrDefaultAsync(m => m.Id == id);
        if (song == null)
        {
            return NotFound();
        }

        return View(song);
    }

    // POST: SONGS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var song = await _context.Songs.FindAsync(id);
        if (song != null)
        {
            _context.Songs.Remove(song);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SongExists(int? id)
    {
        return _context.Songs.Any(e => e.Id == id);
    }
}
