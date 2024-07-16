using Swapkill_Backend.Models;
using Swapkill_Backend.Models.Dto;

namespace Swapkill_Backend.Services
{
    public interface IUserProfileService
    {
        Task<List<UserProfileDto>?> GetAllUserProfiles();
        Task<UserProfileDto?> CreateUserProfile(UserProfileDto userProfile);
        Task<UserProfileDto?> GetUserById(string uid);
        Task<UserProfileDto?> UpdateUserProfile(UserProfileDto userProfile, string uid);
        Task<bool> EmailAlreadyRegistered(string email);
        Task<bool> VerifyEmail(string email, string code);
        Task<bool> RequestResetPassword(string email);
        Task<bool> ResetPassword(string newPassword, string code);
        Task<UserProfileDto?> Login(string email, string password);

    }
}
