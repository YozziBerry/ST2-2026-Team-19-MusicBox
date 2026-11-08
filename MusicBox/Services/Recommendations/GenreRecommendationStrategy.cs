using MusicBox.Models;

namespace MusicBox.Services.Recommendations
{
    // Strategy Pattern:
    // Recommends songs based on the user's preferred genre.
    public class GenreRecommendationStrategy : IRecommendationStrategy
    {
        public List<Song> Recommend(
            List<Song> songs,
            string preferredGenre)
        {
            if (string.IsNullOrWhiteSpace(preferredGenre))
            {
                return new List<Song>();
            }

            return songs
                .Where(song =>
                    song.Genre != null &&
                    song.Genre.Equals(
                        preferredGenre,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
