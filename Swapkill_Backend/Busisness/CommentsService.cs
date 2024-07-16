using Swapkill_Backend.Firebase;
using Swapkill_Backend.Models.Dto;
using Swapkill_Backend.Models;
using Swapkill_Backend.Repositories;
using Swapkill_Backend.Services;
using System.Xml.Linq;

namespace Swapkill_Backend.Busisness
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public CommentsService(ICommentsRepository commentsRepository, IUserProfileRepository userProfileRepository)
        {
            _commentsRepository = commentsRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<CommentDto?> CreateComment(CommentDto comment)
        {
            UserProfile author = await _userProfileRepository.GetById(comment.CreatedBy);
            if (author != null)
            {
                Comment newComment = new Comment
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedBy = comment.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                    Description = comment.Description,
                    ServiceId = comment.ServiceId
                };

                bool result = await _commentsRepository.Post(newComment);
                if (result)
                {
                    comment.Id = newComment.Id;
                    UserProfileDto authorDto = new UserProfileDto()
                    {
                        Id = author.Id,
                        Name = author.Name,
                        Email = author.Email,
                        ProfilePhoto = author.ProfilePhoto
                    };

                    comment.Author = authorDto;

                    return comment;
                }
            }
            return null;
        }

        public async Task<bool> DeleteComment(string id)
        {
            bool result = await _commentsRepository.Delete(id);

            if (result)
            {
                return true;
            }

            return false;
        }

        public async Task<List<CommentDto>?> GetAllComments(string serviceId)
        {

            List<Comment> result = await _commentsRepository.Getall(serviceId);
            List<CommentDto> commentsDto = new List<CommentDto>();
            foreach (Comment comment in result)
            {
                UserProfile author = await _userProfileRepository.GetById(comment.CreatedBy);
                UserProfileDto authorDto = new UserProfileDto()
                {
                    Id = author.Id,
                    Name = author.Name,
                    Email = author.Email,
                    ProfilePhoto = author.ProfilePhoto
                };
                commentsDto.Add(new CommentDto
                {
                    Id = comment.Id,
                    CreatedBy = comment.CreatedBy,
                    CreatedAt = comment.CreatedAt,
                    Description = comment.Description,
                    Author = authorDto
                });
            }

            return commentsDto;
        }

        public async Task<CommentDto?> GetCommentById(string id)
        {
            Comment comment = await _commentsRepository.GetCommentById(id);
            if (comment != null)
            {
                UserProfile author = await _userProfileRepository.GetById(comment.CreatedBy);
                UserProfileDto authorDto = new UserProfileDto()
                {
                    Id = author.Id,
                    Name = author.Name,
                    Email = author.Email,
                    ProfilePhoto = author.ProfilePhoto
                };
                CommentDto commentDto = new CommentDto
                {
                    Id = comment.Id,
                    CreatedBy = comment.CreatedBy,
                    CreatedAt = comment.CreatedAt,
                    Description = comment.Description,
                    Author = authorDto

                };

                return commentDto;
            }

            return null;
        }
    }
}
