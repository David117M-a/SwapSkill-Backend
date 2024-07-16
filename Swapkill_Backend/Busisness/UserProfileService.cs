using Swapkill_Backend.Models;
using Swapkill_Backend.Models.Dto;
using Swapkill_Backend.Services;
using System.Security.Cryptography;
using System.Text;

namespace Swapkill_Backend.Busisness
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public UserProfileService(IUserProfileRepository userProfileRepository, IEmailService emailService, ITokenService tokenService)
        {
            _userProfileRepository = userProfileRepository;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<UserProfileDto?> CreateUserProfile(UserProfileDto userProfile)
        {
            string code = await _emailService.SendVerificationEmail(userProfile.Email, userProfile.Name);
            if (!string.IsNullOrEmpty(code))
            {
                using var haac = new HMACSHA512();
                UserProfile newUser = new UserProfile
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = userProfile.Name,
                    Password = haac.ComputeHash(Encoding.UTF8.GetBytes(userProfile.Password)),
                    PasswordSalt = haac.Key,
                    Email = userProfile.Email,
                    ProfilePhoto = userProfile.ProfilePhoto,
                    IsVerified = false,
                    ResetCode = code
                };

                bool result = await _userProfileRepository.Post(newUser);
                if (result)
                {
                    userProfile.Id = newUser.Id;
                    userProfile.Password = "";
                    return userProfile;
                }
            }

            return null;
        }

        public async Task<UserProfileDto?> UpdateUserProfile(UserProfileDto userProfile, string uid)
        {
            UserProfile oldUser = await _userProfileRepository.GetById(uid);
            using var haac = new HMACSHA512();
            UserProfile newUser = new UserProfile
            {
                Id = uid,
                Name = userProfile.Name,
                Password = oldUser.Password,
                PasswordSalt = oldUser.PasswordSalt,
                Email = userProfile.Email,
                ProfilePhoto = userProfile.ProfilePhoto
            };
            if (oldUser.Email != userProfile.Email)
            {
                string code = await _emailService.SendVerificationEmail(userProfile.Email, userProfile.Name);
                newUser.IsVerified = false;
                newUser.ResetCode = code;
            }

            bool result = await _userProfileRepository.Put(newUser);
            if (result)
            {
                userProfile.Id = newUser.Id;
                return userProfile;
            }

            return null;
        }

        public async Task<List<UserProfileDto>?> GetAllUserProfiles()
        {
            List<UserProfile> result = await _userProfileRepository.GetAll();
            List<UserProfileDto> usersDto = new List<UserProfileDto>();
            foreach (UserProfile userProfile in result)
            {
                usersDto.Add(new UserProfileDto
                {
                    Id = userProfile.Id,
                    Name = userProfile.Name,
                    Email = userProfile.Email,
                    Password = "",
                    ProfilePhoto = userProfile.ProfilePhoto
                });
            }

            return usersDto;
        }

        public async Task<UserProfileDto?> GetUserById(string uid)
        {
            UserProfile result = await _userProfileRepository.GetById(uid);
            if (result != null)
            {
                UserProfileDto usersDto = new UserProfileDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    Email = result.Email,
                    Password = "",
                    ProfilePhoto = result.ProfilePhoto
                };

                return usersDto;
            }

            return null;
        }

        public async Task<bool> EmailAlreadyRegistered(string email)
        {
            List<UserProfile> result = await _userProfileRepository.GetAll();
            UserProfile exists = result.FirstOrDefault(r => r.Email == email);
            if (exists is not null)
                return true;

            return false;
        }

        public async Task<bool> VerifyEmail(string email, string code)
        {
            List<UserProfile> users = await _userProfileRepository.GetAll();
            UserProfile user = users.FirstOrDefault(r => r.Email == email);
            if (user != null)
            {
                if (user.ResetCode == code)
                {
                    user.IsVerified = true;
                    user.ResetCode = null;
                    bool result = await _userProfileRepository.Put(user);
                    if (result)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<bool> RequestResetPassword(string email)
        {
            List<UserProfile> users = await _userProfileRepository.GetAll();
            UserProfile user = users.FirstOrDefault(r => r.Email == email);
            if (user != null)
            {
                if (user.IsVerified)
                {
                    string code = await _emailService.SendResetPasswordEmail(email, user.Name);
                    if (!string.IsNullOrEmpty(code))
                    {
                        user.ResetCode = code;
                        bool result = await _userProfileRepository.Put(user);
                        if (result)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public async Task<UserProfileDto?> Login(string email, string password)
        {
            List<UserProfile> users = await _userProfileRepository.GetAll();
            UserProfile user = users.FirstOrDefault(r => r.Email == email);
            if (user != null)
            {
                if (user.IsVerified)
                {
                    using var hmac = new HMACSHA512(user.PasswordSalt);
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != user.Password[i])
                        {
                            return null;
                        }
                    }

                    string token = _tokenService.CreateToken(user);
                    if (string.IsNullOrEmpty(token))
                        return null;

                    UserProfileDto userDto = new UserProfileDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Password = "",
                        ProfilePhoto = user.ProfilePhoto,
                        Token = token
                    };

                    return userDto;
                }
            }

            return null;
        }

        public async Task<bool> ResetPassword(string newPassword, string code)
        {
            List<UserProfile> users = await _userProfileRepository.GetAll();
            UserProfile user = users.FirstOrDefault(r => r.ResetCode == code);
            if (user != null)
            {
                if (user.IsVerified)
                {
                    using var haac = new HMACSHA512();
                    user.Password = haac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                    user.PasswordSalt = haac.Key;
                    user.ResetCode = "";
                    bool result = await _userProfileRepository.Put(user);
                    if (result)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
