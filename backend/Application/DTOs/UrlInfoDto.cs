namespace backend.DTOs
{
    public class UrlInfoDto
    {
        public int Id { get; set; }
        public required string OriginalUrl { get; set; }
        public required string ShortUrl { get; set; } 
        public DateTime CreatedDate { get; set; }
        public required string CreatedByEmail { get; set; }
    }
}
