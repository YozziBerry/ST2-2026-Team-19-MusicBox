
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicBox.Data;

namespace MusicBox.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly MusicBoxDbContext _context;

        public ArtistsController(MusicBoxDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var artists = await _context.Songs
                .Where(s => !string.IsNullOrEmpty(s.Artist))
                .Select(s => s.Artist)
                .Distinct()
                .OrderBy(a => a)
                .ToListAsync();

            return View(artists);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var songs = await _context.Songs
                .Where(s => s.Artist == id)
                .ToListAsync();

            if (!songs.Any())
            {
                return NotFound();
            }

            return View(songs);
        }
    }
}

