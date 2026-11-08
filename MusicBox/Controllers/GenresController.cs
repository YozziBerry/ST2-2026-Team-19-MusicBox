
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicBox.Data;

namespace MusicBox.Controllers
{
    public class GenresController : Controller
    {
        private readonly MusicBoxDbContext _context;

        public GenresController(MusicBoxDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _context.Songs
                .Where(s => !string.IsNullOrEmpty(s.Genre))
                .Select(s => s.Genre)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();

            return View(genres);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var songs = await _context.Songs
                .Where(s => s.Genre == id)
                .ToListAsync();

            if (!songs.Any())
            {
                return NotFound();
            }

            return View(songs);
        }
    }
}
