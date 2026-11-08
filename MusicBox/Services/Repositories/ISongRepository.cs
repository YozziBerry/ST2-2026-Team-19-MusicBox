using MusicBox.Models;

namespace MusicBox.Services.Repositories
{
    public interface ISongRepository
    {
        // READ: Get all songs
        Task<List<Song>> GetAllAsync();

        // READ: Get a song by ID
        Task<Song?> GetByIdAsync(int id);

        // CREATE: Add a new song
        Task AddAsync(Song song);

        // UPDATE: Update an existing song
        Task UpdateAsync(Song song);

        // DELETE: Remove a song
        Task DeleteAsync(int id);
    }
}
