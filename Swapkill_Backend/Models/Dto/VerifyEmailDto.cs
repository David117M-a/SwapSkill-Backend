namespace Swapkill_Backend.Models.Dto
{
    public class VerifyEmailDto
    {
        public string Email { get; set; }
        public string? Code { get; set; }
    }
}
