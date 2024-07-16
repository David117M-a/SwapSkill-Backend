using Swapkill_Backend.Models;

namespace Swapkill_Backend.Services
{
    public interface IUserProfileRepository
    {
        Task<bool> Post(UserProfile userProfile);
        Task<bool> Put(UserProfile userProfile);
        Task<bool> Delete(string guid);
        Task<List<UserProfile>> GetAll();
        Task<UserProfile> GetById(string uid);
    }
}
