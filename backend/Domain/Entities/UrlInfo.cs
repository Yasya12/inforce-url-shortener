using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class UrlInfo
    {
        public int Id { get; set; }

        [Required]
        public required string OriginalUrl { get; set; }

        [Required]
        public string ShortCode { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Зв'язок з користувачем, який створив посилання
        public  string CreatedById { get; set; }
        public  ApplicationUser CreatedBy { get; set; }
    }
}
