using MusicBox.Models;

namespace MusicBox.Services.Recommendations
{
    // Strategy Pattern:
    // Recommends songs based on the user's preferred artist.
    public class ArtistRecommendationStrategy : IRecommendationStrategy
    {
        public List<Song> Recommend(
            List<Song> songs,
            string preferredArtist)
        {
            if (string.IsNullOrWhiteSpace(preferredArtist))
            {
                return new List<Song>();
            }

            return songs
                .Where(song =>
                    song.Artist.Equals(
                        preferredArtist,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
