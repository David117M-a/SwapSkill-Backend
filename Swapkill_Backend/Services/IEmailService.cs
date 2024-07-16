namespace Swapkill_Backend.Services
{
    public interface IEmailService
    {
        Task<string> SendVerificationEmail(string email, string name);
        Task<string> SendResetPasswordEmail(string email, string name);
    }
}
