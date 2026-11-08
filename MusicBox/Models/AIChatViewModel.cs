using System.ComponentModel.DataAnnotations;

namespace MusicBox.Models
{
    public class AIChatViewModel
    {
        [Required(ErrorMessage = "Моля, въведи въпрос.")]
        [StringLength(
            1000,
            MinimumLength = 3,
            ErrorMessage =
                "Въпросът трябва да бъде между 3 и 1000 символа."
        )]
        public string Question { get; set; } = string.Empty;

        public string? Answer { get; set; }
    }
}