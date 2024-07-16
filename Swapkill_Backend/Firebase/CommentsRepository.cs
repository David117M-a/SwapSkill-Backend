using FireSharp.Response;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Swapkill_Backend.Models;
using Swapkill_Backend.Repositories;

namespace Swapkill_Backend.Firebase
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly FirebaseService _firebaseService;
        private readonly IConfiguration _configuration;
        public CommentsRepository(FirebaseService firebaseService, IConfiguration configuration)
        {
            _firebaseService = firebaseService;
            _configuration = configuration;
        }


        public async Task<bool> Post(Comment comment)
        {
            SetResponse result = await _firebaseService.Client.SetTaskAsync($"{_configuration["DatabasePath:CommentsPath"]}/{comment.Id}", comment);
            if (result.Exception is null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(string id)
        {
            FirebaseResponse result = await _firebaseService.Client.DeleteTaskAsync($"{_configuration["DatabasePath:CommentsPath"]}/{id}");
            if (result.Exception is null)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Comment>> Getall(string serviceId)
        {
            FirebaseResponse result = await _firebaseService.Client.GetTaskAsync($"{_configuration["DatabasePath:CommentsPath"]}");
            if (result.Exception is null)
            {
                dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Body);
                if (data != null)
                {
                    List<Comment> comments = new List<Comment>();
                    foreach (var item in data)
                    {
                        comments.Add(JsonConvert.DeserializeObject<Comment>(((JProperty)item).Value.ToString()));
                    }

                    return comments.Where(c => c.ServiceId == serviceId).OrderByDescending(c => c.CreatedAt).ToList();
                }

                return new List<Comment>();
            }

            return null;
        }

        public async Task<bool> Put(Comment comment)
        {
            FirebaseResponse result = await _firebaseService.Client.UpdateTaskAsync($"{_configuration["DatabasePath:CommentsPath"]}/{comment.Id}", comment);
            if (result.Exception is null)
            {
                return true;
            }

            return false;
        }
        public async Task<Comment> GetCommentById(string id)
        {
            FirebaseResponse result = await _firebaseService.Client.GetTaskAsync($"{_configuration["DatabasePath:CommentsPath"]}");
            if (result.Exception is null)
            {
                dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Body);
                List<Comment> comments = new List<Comment>();
                foreach (var item in data)
                {
                    comments.Add(JsonConvert.DeserializeObject<Comment>(((JProperty)item).Value.ToString()));
                }

                return comments.FirstOrDefault(u => u.Id == id);
            }

            return null;
        }
    }
}
