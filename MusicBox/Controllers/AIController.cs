using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MusicBox.Data;
using MusicBox.Models;
using MusicBox.Services;

namespace MusicBox.Controllers
{
    public class AIController : Controller
    {
        private readonly IAIService _aiService;

        private readonly MusicBoxDbContext _context;

        private readonly ILogger<AIController> _logger;

        public AIController(
            IAIService aiService,
            MusicBoxDbContext context,
            ILogger<AIController> logger)
        {
            _aiService = aiService;
            _context = context;
            _logger = logger;
        }

        // Opens the AI Assistant page
        [HttpGet]
        public IActionResult Index()
        {
            return View(new AIChatViewModel());
        }

        // Sends the question to Gemini
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ask(
            AIChatViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                // Retrieve songs from the database
                var songs = await _context.Songs
                    .AsNoTracking()
                    .OrderBy(song => song.Title)
                    .Take(50)
                    .ToListAsync(cancellationToken);

                // Convert songs into text
                string musicCatalogue;

                if (songs.Count == 0)
                {
                    musicCatalogue =
                        "The MusicBox catalogue is currently empty.";
                }
                else
                {
                    musicCatalogue = string.Join(
                        Environment.NewLine,

                        songs.Select(song =>
                            $"- {song.Title} | " +
                            $"{song.Artist} | " +
                            $"{song.Genre} | " +
                            $"{song.ReleaseYear}"
                        )
                    );
                }

                // Ask Gemini to generate an answer
                model.Answer = await _aiService.GetAnswerAsync(
                    model.Question,
                    musicCatalogue,
                    cancellationToken
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error communicating with Gemini AI."
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Възникна проблем при комуникацията с AI. " +
                    "Провери API настройките и опитай отново."
                );
            }

            return View("Index", model);
        }
    }
}