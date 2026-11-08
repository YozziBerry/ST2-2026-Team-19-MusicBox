using Microsoft.AspNetCore.Mvc;
using MusicBox.Models;
using MusicBox.Services;
using MusicBox.Services.Repositories;

namespace MusicBox.Controllers
{
    public class SongsController : Controller
    {
        private readonly ISongRepository _songRepository;

        public SongsController(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        // READ: Display all songs and filter by title, artist or genre
        public async Task<IActionResult> Index(string? searchString)
        {
            var songs = await _songRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                songs = songs.Where(s =>
                    s.Title.Contains(searchString,
                        StringComparison.OrdinalIgnoreCase) ||
                    s.Artist.Contains(searchString,
                        StringComparison.OrdinalIgnoreCase) ||
                    (s.Genre != null &&
                     s.Genre.Contains(searchString,
                        StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            ViewData["CurrentFilter"] = searchString;

            // Singleton Design Pattern: use the shared visit counter
            ViewData["VisitCount"] =
                MusicBoxVisitCounter.Instance.RegisterVisit();

            return View(songs);
        }

        // READ: Display details of a specific song
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var song = await _songRepository.GetByIdAsync(id.Value);

            if (song == null) return NotFound();

            return View(song);
        }

        // CREATE: Display form
        public IActionResult Create()
        {
            return View();
        }

        // CREATE: Save new song
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Artist,Genre,ReleaseYear")] Song song)
        {
            if (!ModelState.IsValid) return View(song);

            await _songRepository.AddAsync(song);

            return RedirectToAction(nameof(Index));
        }

        // UPDATE: Display edit form
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var song = await _songRepository.GetByIdAsync(id.Value);

            if (song == null) return NotFound();

            return View(song);
        }

        // UPDATE: Save changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Artist,Genre,ReleaseYear")] Song song)
        {
            if (id != song.Id) return NotFound();

            if (!ModelState.IsValid) return View(song);

            var existingSong =
                await _songRepository.GetByIdAsync(id);

            if (existingSong == null) return NotFound();

            existingSong.Title = song.Title;
            existingSong.Artist = song.Artist;
            existingSong.Genre = song.Genre;
            existingSong.ReleaseYear = song.ReleaseYear;

            await _songRepository.UpdateAsync(existingSong);

            return RedirectToAction(nameof(Index));
        }

        // DELETE: Confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var song = await _songRepository.GetByIdAsync(id.Value);

            if (song == null) return NotFound();

            return View(song);
        }

        // DELETE: Remove song
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _songRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
