using FireSharp.Config;
using FireSharp.Interfaces;

namespace Swapkill_Backend.Firebase
{
    public class FirebaseService
    {
        public IFirebaseClient Client { get; set; }
        public FirebaseService(string authSecret, string basePath)
        {
            if (string.IsNullOrEmpty(authSecret) || string.IsNullOrEmpty(basePath))
            {
                throw new Exception("Firebase configuration is not complete");
            }

            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = authSecret,
                BasePath = basePath
            };

            Client = new FireSharp.FirebaseClient(config);
            if (Client == null)
            {
                throw new Exception("Could not connect to database");
            }
        }
    }
}
