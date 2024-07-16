using FireSharp.Response;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Swapkill_Backend.Models;
using Swapkill_Backend.Repositories;
using System.Net;

namespace Swapkill_Backend.Firebase
{
    public class ServicePostRepository : IServicePostRepository
    {
        private readonly FirebaseService _firebaseService;
        private readonly IConfiguration _configuration;
        public ServicePostRepository(FirebaseService firebaseService, IConfiguration configuration)
        {
            _firebaseService = firebaseService;
            _configuration = configuration;
        }


        public async Task<bool> Post(ServicePost servicePost)
        {
            SetResponse result = await _firebaseService.Client.SetTaskAsync($"{_configuration["DatabasePath:ServicePostPath"]}/{servicePost.Id}", servicePost);
            if (result.Exception is null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(string id)
        {
            FirebaseResponse result = await _firebaseService.Client.DeleteTaskAsync($"{_configuration["DatabasePath:ServicePostPath"]}/{id}");
            if (result.Exception is null)
            {
                return true;
            }
            return false;
        }

        public async Task<List<ServicePost>> Getall()
        {
            FirebaseResponse result = await _firebaseService.Client.GetTaskAsync($"{_configuration["DatabasePath:ServicePostPath"]}");
            if (result.Exception is null)
            {
                dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Body);
                if (data != null)
                {
                    List<ServicePost> users = new List<ServicePost>();
                    foreach (var item in data)
                    {
                        users.Add(JsonConvert.DeserializeObject<ServicePost>(((JProperty)item).Value.ToString()));
                    }

                    return users.OrderByDescending(s => s.CreatedAt).ToList();
                }

                return new List<ServicePost>();
            }

            return null;
        }

        public async Task<bool> Put(ServicePost servicePost)
        {
            FirebaseResponse result = await _firebaseService.Client.UpdateTaskAsync($"{_configuration["DatabasePath:ServicePostPath"]}/{servicePost.Id}", servicePost);
            if (result.Exception is null)
            {
                return true;
            }

            return false;
        }
        public async Task<ServicePost> GetServiceById(string id)
        {
            FirebaseResponse result = await _firebaseService.Client.GetTaskAsync($"{_configuration["DatabasePath:ServicePostPath"]}");
            if (result.Exception is null)
            {
                dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Body);
                List<ServicePost> users = new List<ServicePost>();
                foreach (var item in data)
                {
                    users.Add(JsonConvert.DeserializeObject<ServicePost>(((JProperty)item).Value.ToString()));
                }

                return users.FirstOrDefault(u => u.Id == id);
            }

            return null;
        }



        public async Task<List<ServicePost>> GetCreateby(string id)
        {
            FirebaseResponse result = await _firebaseService.Client.GetTaskAsync($"{_configuration["DatabasePath:ServicePostPath"]}");
            if (result.Exception is null)
            {
                dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Body);
                List<ServicePost> posts = new List<ServicePost>();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        posts.Add(JsonConvert.DeserializeObject<ServicePost>(((JProperty)item).Value.ToString()));
                    }

                    return posts.Where(u => u.CreatedBy == id).ToList();
                }

                return posts;
            }

            return null;
        }

    }
}
