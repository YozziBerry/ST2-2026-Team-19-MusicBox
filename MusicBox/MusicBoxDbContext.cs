using Microsoft.EntityFrameworkCore;
using MusicBox.Models;

namespace MusicBox.Data
{
    public class MusicBoxDbContext : DbContext
    {
        public MusicBoxDbContext(
            DbContextOptions<MusicBoxDbContext> options)
            : base(options)
        {
        }

        public DbSet<Song> Songs { get; set; }
    }
}
