using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BioSportApp.Common.Security
{
    public class PasswordUtils
    {
        public static PasswordHash PasswordHash(string password)
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));

            return new PasswordHash
            {
                Hash = hashed,
                Salt = Convert.ToBase64String(salt)
            };
        }

        public static string PasswordHash(string password, string salt)
        {
            var saltByteArray = Convert.FromBase64String(salt);

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                saltByteArray,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));

            return hashed;
        }
    }
}
