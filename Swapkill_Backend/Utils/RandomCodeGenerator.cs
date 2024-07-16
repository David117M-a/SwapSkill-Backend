using System.Text;

namespace Swapkill_Backend.Utils
{
    public class RandomCodeGenerator
    {
        private static Random random = new Random();

        public static string GenerateRandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder codeBuilder = new StringBuilder(8);

            for (int i = 0; i < 8; i++)
            {
                codeBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return codeBuilder.ToString();
        }
    }
}
