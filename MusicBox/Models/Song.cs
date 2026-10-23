using System.ComponentModel.DataAnnotations;

namespace MusicBox.Models
{
    public class Song
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Artist { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Genre { get; set; }

        [Range(1000, 2100)]
        public int? ReleaseYear { get; set; }
    }
}
