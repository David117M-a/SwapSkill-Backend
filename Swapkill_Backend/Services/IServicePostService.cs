using Swapkill_Backend.Models.Dto;

namespace Swapkill_Backend.Services
{
    public interface IServicePostService
    {
        Task<List<ServicePostDto>?> GetAllServices();
        Task<ServicePostDto?> CreateService(ServicePostDto servicePost);
        Task<ServicePostDto> GetServiceById(string id);
        Task<ServicePostDto?> UpdateServiceProfile(ServicePostDto servicePost, string id);

        Task<List<ServicePostDto>?> GetCreateBy(string id);

        Task<bool> DeleteService(string id);
        Task<List<ServicePostDto>> GetFilteredServices(string? serviceName, string? location);

    }
}
