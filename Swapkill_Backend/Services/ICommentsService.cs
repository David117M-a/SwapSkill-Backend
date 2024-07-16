using Swapkill_Backend.Models.Dto;

namespace Swapkill_Backend.Services
{
    public interface ICommentsService
    {
        Task<CommentDto?> CreateComment(CommentDto comment);
        Task<bool> DeleteComment(string id);
        Task<List<CommentDto>?> GetAllComments(string serviceId);
        Task<CommentDto?> GetCommentById(string id);
    }
}
