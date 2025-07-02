using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CreateUrlDto
    {
        [Required]
        [Url] 
        public required string OriginalUrl { get; set; }
    }
}
