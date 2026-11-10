
using System.Collections.Generic;

namespace MusicBox.Models
{
    public class RecommendationViewModel
    {
        public string Strategy { get; set; } = "Genre";

        public string Preference { get; set; } = string.Empty;

        public List<Song> Recommendations { get; set; } = new List<Song>();
    }
}

