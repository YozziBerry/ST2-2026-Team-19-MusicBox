
using MusicBox.Models;

namespace MusicBox.Services.Recommendations
{
    // Strategy Pattern:
    // Recommends songs based on the user's preferred genre.
    public class GenreRecommendationStrategy : IRecommendationStrategy
    {
        public string Name => "Genre";

        public List<Song> Recommend(
            List<Song> songs,
            string preference)
        {
            if (string.IsNullOrWhiteSpace(preference))
            {
                return new List<Song>();
            }

            return songs
                .Where(song =>
                    !string.IsNullOrWhiteSpace(song.Genre) &&
                    song.Genre.Equals(
                        preference,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}

