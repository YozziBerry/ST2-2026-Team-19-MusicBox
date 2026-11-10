
using MusicBox.Models;

namespace MusicBox.Services.Recommendations
{
    // Strategy Pattern:
    // Recommends songs based on the user's preferred artist.
    public class ArtistRecommendationStrategy : IRecommendationStrategy
    {
        public string Name => "Artist";

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
                    !string.IsNullOrWhiteSpace(song.Artist) &&
                    song.Artist.Equals(
                        preference,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}

