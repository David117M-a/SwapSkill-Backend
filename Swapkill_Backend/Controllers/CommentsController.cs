using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swapkill_Backend.Busisness;
using Swapkill_Backend.Models.Dto;
using Swapkill_Backend.Services;

namespace Swapkill_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService, IConfiguration configuration)
        {
            _commentsService = commentsService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> CreateComment([FromBody] CommentDto comment)
        {
            try
            {
                if (string.IsNullOrEmpty(comment.CreatedBy))
                {
                    return BadRequest(new { message = "CreatedBy is required" });
                }

                if (string.IsNullOrEmpty(comment.Description))
                {
                    return BadRequest(new { message = "Description is required" });
                }

                if (string.IsNullOrEmpty(comment.ServiceId))
                {
                    return BadRequest(new { message = "ServiceId is required" });
                }

                CommentDto result = await _commentsService.CreateComment(comment);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { message = "Comment could not be published" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong, contact administrator" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(string id)
        {
            bool result = await _commentsService.DeleteComment(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest(new { message = "Comment could not be deleted" });
        }

        [HttpGet]
        public async Task<List<CommentDto>> GetAllComments([FromQuery] string serviceId)
        {
            return await _commentsService.GetAllComments(serviceId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCommentById(string id)
        {
            var result = await _commentsService.GetCommentById(id);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }
    }
}
