namespace Swapkill_Backend.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public string ServiceId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
