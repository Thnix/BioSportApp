using System.Security.Cryptography;
using System.Text;

namespace BioSportApp.Common.Security
{
    public static class Cryptography
    {
        /// <summary>
        /// Creates a cryptographically secure random key string.
        /// </summary>
        /// <param name="count">The number of bytes of random values to create the string from</param>
        /// <returns>A secure random string</returns>
        public static string CreateSecureRandomString(int count = 64) =>
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(count));


        public static string ToSHA512(this string value)
        {
            using var sha = SHA512.Create();

            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
