using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Diagnostics;

namespace Swapkill_Backend.Brevo
{
    public class BrevoService
    {
        public TransactionalEmailsApi ApiInstance { get; set; }

        public BrevoService(string apiKey)
        {

            try
            {
                // Configure API key authorization: api-key
                Configuration.Default.ApiKey.Add("api-key", apiKey);

                ApiInstance = new TransactionalEmailsApi();
                Debug.WriteLine("Brevo service up!");
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling Brevo service: " + e.Message);
            }
        }
    }
}
