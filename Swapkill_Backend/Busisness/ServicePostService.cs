using Swapkill_Backend.Firebase;
using Swapkill_Backend.Models;
using Swapkill_Backend.Models.Dto;
using Swapkill_Backend.Repositories;
using Swapkill_Backend.Services;
using System.Net;

namespace Swapkill_Backend.Busisness
{
    public class ServicePostService : IServicePostService
    {
        private readonly IServicePostRepository _servicePostRepository;


        public ServicePostService(IServicePostRepository servicePostRepository)
        {
            _servicePostRepository = servicePostRepository;
        }

        public async Task<ServicePostDto?> CreateService(ServicePostDto servicePost)
        {
            ServicePost newservice = new ServicePost
            {
                Id = Guid.NewGuid().ToString(),
                Detail = servicePost.Detail,
                Image = servicePost.Image,
                Location = servicePost.Location,
                Title = servicePost.Title,
                CreatedBy = servicePost.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            bool result = await _servicePostRepository.Post(newservice);
            if (result)
            {
                servicePost.Id = newservice.Id;

                return servicePost;
            }


            return null;
        }

        public async Task<bool> DeleteService(string id)
        {
            bool result = await _servicePostRepository.Delete(id);

            if (result)
            {
                return true;
            }

            return false;
        }

        public async Task<List<ServicePostDto>?> GetAllServices()
        {

            List<ServicePost> result = await _servicePostRepository.Getall();
            List<ServicePostDto> serviceDto = new List<ServicePostDto>();
            foreach (ServicePost servicePost in result)
            {
                serviceDto.Add(new ServicePostDto
                {
                    Id = servicePost.Id,
                    Detail = servicePost.Detail,
                    Image = servicePost.Image,
                    Location = servicePost.Location,
                    Title = servicePost.Title,
                    CreatedBy = servicePost.CreatedBy,
                    CreatedAt = servicePost.CreatedAt
                });
            }

            return serviceDto;
        }

        public async Task<List<ServicePostDto?>> GetCreateBy(string id)
        {
            List<ServicePost> result = await _servicePostRepository.GetCreateby(id);
            List<ServicePostDto> serviceDto = new List<ServicePostDto>();
            foreach (ServicePost servicePost in result)
            {
                serviceDto.Add(new ServicePostDto
                {
                    Id = servicePost.Id,
                    Detail = servicePost.Detail,
                    Image = servicePost.Image,
                    Location = servicePost.Location,
                    Title = servicePost.Title,
                    CreatedBy = servicePost.CreatedBy,
                    CreatedAt = servicePost.CreatedAt
                });
            }

            return serviceDto;
        }

        public async Task<ServicePostDto?> GetServiceById(string id)
        {
            ServicePost result = await _servicePostRepository.GetServiceById(id);
            if (result != null)
            {
                ServicePostDto usersDto = new ServicePostDto
                {
                    Id = result.Id,
                    Detail = result.Detail,
                    Image = result.Image,
                    Location = result.Location,
                    Title = result.Title,
                    CreatedBy = result.CreatedBy,
                    CreatedAt = result.CreatedAt,

                };

                return usersDto;
            }

            return null;
        }

        public async Task<ServicePostDto?> UpdateServiceProfile(ServicePostDto servicePost, string id)
        {
            ServicePost oldServicePost = await _servicePostRepository.GetServiceById(id);

            oldServicePost.Detail = servicePost.Detail;
            oldServicePost.Image = servicePost.Image;
            oldServicePost.Location = servicePost.Location;
            oldServicePost.Title = servicePost.Title;
            bool result = await _servicePostRepository.Put(oldServicePost);
            if (result)
            {
                servicePost.CreatedBy = oldServicePost.CreatedBy;
                servicePost.Id = oldServicePost.Id;
                return servicePost;
            }

            return null;
        }

        public async Task<List<ServicePostDto>> GetFilteredServices(string? serviceName, string? location)
        {

            List<ServicePost> result = await _servicePostRepository.Getall();
            List<ServicePostDto> serviceDto = new List<ServicePostDto>();
            foreach (ServicePost servicePost in result)
            {
                if (!string.IsNullOrEmpty(serviceName) && !string.IsNullOrEmpty(location))
                {
                    if (servicePost.Title.IndexOf(serviceName, StringComparison.OrdinalIgnoreCase) >= 0 && servicePost.Location.IndexOf(location, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        serviceDto.Add(new ServicePostDto
                        {
                            Id = servicePost.Id,
                            Detail = servicePost.Detail,
                            Image = servicePost.Image,
                            Location = servicePost.Location,
                            Title = servicePost.Title,
                            CreatedBy = servicePost.CreatedBy,
                            CreatedAt = servicePost.CreatedAt,
                        });
                    }
                }

                if (!string.IsNullOrEmpty(serviceName) && string.IsNullOrEmpty(location))
                {
                    if (servicePost.Title.IndexOf(serviceName, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        serviceDto.Add(new ServicePostDto
                        {
                            Id = servicePost.Id,
                            Detail = servicePost.Detail,
                            Image = servicePost.Image,
                            Location = servicePost.Location,
                            Title = servicePost.Title,
                            CreatedBy = servicePost.CreatedBy,
                            CreatedAt = servicePost.CreatedAt,
                        });
                    }
                }

                if (!string.IsNullOrEmpty(location) && string.IsNullOrEmpty(serviceName))
                {
                    if (servicePost.Location.IndexOf(location, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        serviceDto.Add(new ServicePostDto
                        {
                            Id = servicePost.Id,
                            Detail = servicePost.Detail,
                            Image = servicePost.Image,
                            Location = servicePost.Location,
                            Title = servicePost.Title,
                            CreatedBy = servicePost.CreatedBy,
                            CreatedAt = servicePost.CreatedAt
                        });
                    }
                }
            }

            return serviceDto;
        }
    }
}
