namespace Swapkill_Backend.Models
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? Token { get; set; }
        public string? ResetCode { get; set; }
        public bool IsVerified { get; set; } = false;
    }
}
