using MusicBox.Models;

namespace MusicBox.Services.Recommendations
{
    // Strategy Pattern:
    // Defines a common interface for different
    // song recommendation strategies.
    public interface IRecommendationStrategy
    {
        List<Song> Recommend(
            List<Song> songs,
            string preferredGenre);
    }
}
