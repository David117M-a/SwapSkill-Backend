using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swapkill_Backend.Models;
using Swapkill_Backend.Models.Dto;
using Swapkill_Backend.Services;
using System.Security.Cryptography;
using System.Text;

namespace Swapkill_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IConfiguration _configuration;

        public UserProfileController(IUserProfileService userProfileService, IConfiguration configuration)
        {
            _userProfileService=userProfileService;
            _configuration=configuration;
        }

        [HttpGet]
        public async Task<List<UserProfileDto>> GetAllUsers()
        {
            return await _userProfileService.GetAllUserProfiles();
        }

        [HttpGet("{uid}")]
        public async Task<ActionResult> GetUserById(string uid)
        {
            var result = await _userProfileService.GetUserById(uid);
            if(result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateUserProfile([FromBody] UserProfileDto userProfile)
        {
            try
            {
                if (string.IsNullOrEmpty(userProfile.Name))
                {
                    return BadRequest(new { message = "Name is required" });
                }

                if (string.IsNullOrEmpty(userProfile.Email))
                {
                    return BadRequest(new { message = "Email is required" });
                }

                if (string.IsNullOrEmpty(userProfile.Password))
                {
                    return BadRequest(new { message = "Password is required" });
                }

                if (await _userProfileService.EmailAlreadyRegistered(userProfile.Email))
                {
                    return BadRequest(new { message = "Email is already registered" });
                }

                UserProfileDto result = await _userProfileService.CreateUserProfile(userProfile);
                if (result == null)
                {
                    return BadRequest(new { message = "User profile could not been created" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong, contact administrator" });
            }
        }

        [HttpPost("VerifyEmail")]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyEmail([FromBody] VerifyEmailDto verifyEmailDto)
        {
            try
            {
                if (string.IsNullOrEmpty(verifyEmailDto.Email))
                {
                    return BadRequest(new { message = "Email is required" });
                }

                if (string.IsNullOrEmpty(verifyEmailDto.Code))
                {
                    return BadRequest(new { message = "Code is required" });
                }

                bool result = await _userProfileService.VerifyEmail(verifyEmailDto.Email, verifyEmailDto.Code);
                if (result)
                    return Ok(new { message = "Email verified" });

                return BadRequest(new { message = "Email or code not valid" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong, contact administrator" });
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] UserProfileDto userProfileDto)
        {
            try
            {
                if (string.IsNullOrEmpty(userProfileDto.Email))
                {
                    return BadRequest(new { message = "Email is required" });
                }

                if (string.IsNullOrEmpty(userProfileDto.Password))
                {
                    return BadRequest(new { message = "Password is required" });
                }

                UserProfileDto user = await _userProfileService.Login(userProfileDto.Email, userProfileDto.Password);
                if (user != null)
                {
                    return Ok(user);
                }

                return Unauthorized(new { message = "Email or password not valid" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong, contact administrator" });
            }
        }

        [HttpPost("RequestResetPassword")]
        [AllowAnonymous]
        public async Task<ActionResult> RequestResetPassword([FromBody] VerifyEmailDto verifyEmailDto)
        {
            try
            {
                if (string.IsNullOrEmpty(verifyEmailDto.Email))
                {
                    return BadRequest(new { message = "Email is required" });
                }

                bool result = await _userProfileService.RequestResetPassword(verifyEmailDto.Email);
                if (result)
                    return Ok(new { message = "Reset Password requested" });

                return BadRequest(new { message = "Email or User not valid" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong, contact administrator" });
            }
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                if (string.IsNullOrEmpty(resetPasswordDto.Code))
                {
                    return BadRequest(new { message = "Code is required" });
                }

                if (string.IsNullOrEmpty(resetPasswordDto.NewPassword))
                {
                    return BadRequest(new { message = "NewPassword is required" });
                }

                bool result = await _userProfileService.ResetPassword(resetPasswordDto.NewPassword, resetPasswordDto.Code);
                if (result)
                    return Ok(new { message = "Password restored succesfully" });

                return BadRequest(new { message = "Code or User not valid" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong, contact administrator" });
            }
        }

        [HttpPut("{uid}")]
        public async Task<ActionResult> UpdateUserProfile([FromBody] UserProfileDto userProfile, string uid)
        {
            try
            {
                if (string.IsNullOrEmpty(userProfile.Name))
                {
                    return BadRequest(new { message = "Name is required" });
                }

                if (string.IsNullOrEmpty(userProfile.Email))
                {
                    return BadRequest(new { message = "Email is required" });
                }

                UserProfileDto result = await _userProfileService.UpdateUserProfile(userProfile, uid);
                if (result == null)
                {
                    return BadRequest(new { message = "User profile could not been created" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong, contact administrator" });
            }
        }
    }
}
