using sib_api_v3_sdk.Model;
using Swapkill_Backend.Services;
using Swapkill_Backend.Utils;
using System.Collections.Generic;
using System.Diagnostics;

namespace Swapkill_Backend.Brevo
{
    public class EmailService : IEmailService
    {
        private readonly BrevoService _brevoService;
        private readonly IConfiguration _configuration;

        public EmailService(BrevoService brevoService, IConfiguration configuration)
        {
            _brevoService = brevoService;
            _configuration = configuration;
        }

        public async Task<string> SendVerificationEmail(string email, string name)
        {
            try
            {
                string code = RandomCodeGenerator.GenerateRandomCode();
                SendSmtpEmail smtpEmail = new SendSmtpEmail
                {
                    To = new List<SendSmtpEmailTo> { new SendSmtpEmailTo(email, name) },
                    TemplateId = int.Parse(_configuration["BrevoConfig:VerificationTemplate"]),
                    Params = new
                    {
                        name = name,
                        code = code
                    }
                };

                var result = await _brevoService.ApiInstance.SendTransacEmailAsync(smtpEmail);
                if (string.IsNullOrEmpty(result.MessageId))
                {
                    return "";
                }

                return code;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return "";
            }
        }

        public async Task<string> SendResetPasswordEmail(string email, string name)
        {
            try
            {
                string code = RandomCodeGenerator.GenerateRandomCode();
                SendSmtpEmail smtpEmail = new SendSmtpEmail
                {
                    To = new List<SendSmtpEmailTo> { new SendSmtpEmailTo(email, name) },
                    TemplateId = int.Parse(_configuration["BrevoConfig:ResetPasswordTemplate"]),
                    Params = new
                    {
                        name = name,
                        code = code
                    }
                };

                var result = await _brevoService.ApiInstance.SendTransacEmailAsync(smtpEmail);
                if (string.IsNullOrEmpty(result.MessageId))
                {
                    return "";
                }

                return code;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return "";
            }
        }
    }
}
