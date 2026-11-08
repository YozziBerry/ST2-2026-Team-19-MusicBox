using Microsoft.EntityFrameworkCore;
using MusicBox.Data;
using MusicBox.Models;

namespace MusicBox.Services.Repositories
{
    // Repository Pattern:
    // This class handles database operations for songs.
    public class SongRepository : ISongRepository
    {
        private readonly MusicBoxDbContext _context;

        public SongRepository(MusicBoxDbContext context)
        {
            _context = context;
        }

        // READ: Get all songs
        public async Task<List<Song>> GetAllAsync()
        {
            return await _context.Songs
                .OrderBy(s => s.Title)
                .ToListAsync();
        }

        // READ: Get a song by ID
        public async Task<Song?> GetByIdAsync(int id)
        {
            return await _context.Songs
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // CREATE: Add a new song
        public async Task AddAsync(Song song)
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
        }

        // UPDATE: Update an existing song
        public async Task UpdateAsync(Song song)
        {
            _context.Songs.Update(song);
            await _context.SaveChangesAsync();
        }

        // DELETE: Remove a song
        public async Task DeleteAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
        }
    }
}
