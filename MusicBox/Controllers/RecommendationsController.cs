
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicBox.Data;
using MusicBox.Models;
using MusicBox.Services.Recommendations;

namespace MusicBox.Controllers
{
    public class RecommendationsController : Controller
    {
        private readonly MusicBoxDbContext _context;
        private readonly IEnumerable<IRecommendationStrategy> _strategies;

        public RecommendationsController(
            MusicBoxDbContext context,
            IEnumerable<IRecommendationStrategy> strategies)
        {
            _context = context;
            _strategies = strategies;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new RecommendationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(RecommendationViewModel model)
        {
            var strategy = _strategies.FirstOrDefault(
                s => s.Name == model.Strategy);

            if (strategy == null)
            {
                ModelState.AddModelError(
                    "Strategy",
                    "Invalid recommendation strategy.");

                return View(model);
            }

            var songs = await _context.Songs.ToListAsync();

            model.Recommendations = strategy.Recommend(
                songs,
                model.Preference);

            return View(model);
        }
    }
}
