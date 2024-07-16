using Swapkill_Backend.Models;

namespace Swapkill_Backend.Services
{
    public interface ITokenService
    {
        string? CreateToken(UserProfile user);
    }
}
