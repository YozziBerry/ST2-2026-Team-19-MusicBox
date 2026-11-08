namespace MusicBox.Services
{
    public interface IAIService
    {
        Task<string> GetAnswerAsync(
            string userQuestion,
            string musicCatalogue,
            CancellationToken cancellationToken = default
        );
    }
}