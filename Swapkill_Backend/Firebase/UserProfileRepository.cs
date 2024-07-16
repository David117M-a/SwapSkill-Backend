using FireSharp.Extensions;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swapkill_Backend.Models;
using Swapkill_Backend.Services;
using System.Text.Json.Serialization;

namespace Swapkill_Backend.Firebase
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly FirebaseService _firebaseService;
        private readonly IConfiguration _configuration;
        public UserProfileRepository(FirebaseService firebaseService, IConfiguration configuration)
        {
            _firebaseService = firebaseService;
            _configuration = configuration;
        }

        public async Task<bool> Post(UserProfile userProfile)
        {
            //PushResponse result = await _firebaseService.Client.PushTaskAsync(_configuration["DatabasePath:UserProfilePath"], userProfile);
            SetResponse result = await _firebaseService.Client.SetTaskAsync($"{_configuration["DatabasePath:UserProfilePath"]}/{userProfile.Id}", userProfile);
            if (result.Exception is null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> Put(UserProfile userProfile)
        {
            FirebaseResponse result = await _firebaseService.Client.UpdateTaskAsync($"{_configuration["DatabasePath:UserProfilePath"]}/{userProfile.Id}", userProfile);
            if (result.Exception is null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(string uid)
        {
            FirebaseResponse result = await _firebaseService.Client.DeleteTaskAsync($"{_configuration["DatabasePath:UserProfilePath"]}/{uid}");
            if (result.Exception is null)
            {
                return true;
            }

            return false;
        }

        public async Task<List<UserProfile>> GetAll()
        {
            FirebaseResponse result = await _firebaseService.Client.GetTaskAsync($"{_configuration["DatabasePath:UserProfilePath"]}");
            if (result.Exception is null)
            {
                dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Body);
                if (data != null)
                {
                    List<UserProfile> users = new List<UserProfile>();
                    foreach (var item in data)
                    {
                        users.Add(JsonConvert.DeserializeObject<UserProfile>(((JProperty)item).Value.ToString()));
                    }

                    return users;
                }

                return new List<UserProfile>();
            }

            return null;
        }

        public async Task<UserProfile> GetById(string uid)
        {
            FirebaseResponse result = await _firebaseService.Client.GetTaskAsync($"{_configuration["DatabasePath:UserProfilePath"]}");
            if (result.Exception is null)
            {
                dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Body);
                List<UserProfile> users = new List<UserProfile>();
                foreach (var item in data)
                {
                    users.Add(JsonConvert.DeserializeObject<UserProfile>(((JProperty)item).Value.ToString()));
                }

                return users.FirstOrDefault(u => u.Id == uid);
            }

            return null;
        }
    }
}
