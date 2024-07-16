using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swapkill_Backend.Busisness;
using Swapkill_Backend.Models.Dto;
using Swapkill_Backend.Models;
using Swapkill_Backend.Repositories;
using Swapkill_Backend.Services;
using System.Net;

namespace Swapkill_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IServicePostService _servicePostService;

        public ServicesController(IServicePostService servicePostService, IConfiguration configuration)
        {
            _servicePostService = servicePostService;
            _configuration = configuration;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteServicePost(string id)
        {
            bool result = await _servicePostService.DeleteService(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest(new { message = "service could not be deleted" });
        }

        [HttpGet]
        public async Task<List<ServicePostDto>> GetAllService()
        {
            return await _servicePostService.GetAllServices();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetServiceById(string id)
        {
            var result = await _servicePostService.GetServiceById(id);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet("byUser/{id}")]
        public async Task<List<ServicePostDto>> GetCreateBy(string id)
        {
            return await _servicePostService.GetCreateBy(id);
        }

        [HttpGet("filtered")]
        public async Task<List<ServicePostDto>> GetCreateBy([FromQuery] string? serviceName, string? city)
        {
            return await _servicePostService.GetFilteredServices(serviceName, city);
        }

        [HttpPost]
        public async Task<ActionResult> CreateServiceProfile([FromBody] ServicePostDto servicePost)
        {
            try
            {
                if (string.IsNullOrEmpty(servicePost.Title))
                {
                    return BadRequest(new { message = "title is required" });
                }

                if (string.IsNullOrEmpty(servicePost.Detail))
                {
                    return BadRequest(new { message = "detail  is required" });
                }

                if (string.IsNullOrEmpty(servicePost.Image))
                {
                    return BadRequest(new { message = "Image is required" });
                }
                if (string.IsNullOrEmpty(servicePost.Location))
                {
                    return BadRequest(new { message = "Location is required" });
                }
                if (string.IsNullOrEmpty(servicePost.CreatedBy))
                {
                    return BadRequest(new { message = "userId is required" });
                }



                ServicePostDto result = await _servicePostService.CreateService(servicePost);


                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong, contact administrator" });
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateService([FromBody] ServicePostDto servicePost, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(servicePost.Title))
                {
                    return BadRequest(new { message = "Title is required" });
                }

                if (string.IsNullOrEmpty(servicePost.Detail))
                {
                    return BadRequest(new { message = "Detail is required" });
                }

                if (string.IsNullOrEmpty(servicePost.Image))
                {
                    return BadRequest(new { message = "Image is required" });
                }
                if (string.IsNullOrEmpty(servicePost.Location))
                {
                    return BadRequest(new { message = "Location is required" });
                }

                ServicePostDto? result = await _servicePostService.UpdateServiceProfile(servicePost, id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong, contact administrator" });
            }
        }
    }
}
