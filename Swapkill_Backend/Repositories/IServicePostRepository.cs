using Swapkill_Backend.Models;

namespace Swapkill_Backend.Repositories
{
    public interface IServicePostRepository
    {
        Task<bool> Post(ServicePost servicePost);
        Task<bool> Put(ServicePost servicePost);
        Task<bool> Delete(string id);
        Task<List<ServicePost>> Getall();
        Task<ServicePost> GetServiceById(string id);

        Task<List<ServicePost>>GetCreateby(string id);
      
    }
}
