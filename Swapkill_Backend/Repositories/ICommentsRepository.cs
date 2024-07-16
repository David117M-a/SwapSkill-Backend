using Swapkill_Backend.Models;

namespace Swapkill_Backend.Repositories
{
    public interface ICommentsRepository
    {
        Task<bool> Post(Comment servicePost);
        Task<bool> Put(Comment servicePost);
        Task<bool> Delete(string id);
        Task<List<Comment>> Getall(string serviceId);
        Task<Comment> GetCommentById(string id);
    }
}
