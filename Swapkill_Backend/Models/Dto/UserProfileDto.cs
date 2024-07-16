namespace Swapkill_Backend.Models.Dto
{
    public class UserProfileDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? Token { get; set; }
    }
}
