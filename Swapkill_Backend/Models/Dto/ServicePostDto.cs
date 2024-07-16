namespace Swapkill_Backend.Models.Dto
{
    public class ServicePostDto
    {
        public string? CreatedBy { get; set; }
        public string? Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
