using System.Security.Cryptography;

namespace UserApi.Helpers
{
    /// <summary>
    /// Provides methods for hashing and validating passwords.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        /// <summary>
        /// Hashes a password using a random salt and SHA512 algorithm.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A tuple containing the hash and salt.</returns>
        public (byte[] hash, byte[] salt) HashPassword(string password)
        {
            // Generate a random salt
            var salt = RandomNumberGenerator.GetBytes(SaltSize);

            // Hash the password and salt using SHA512
            using var sha512 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512);
            var hash = sha512.GetBytes(HashSize);

            return (hash, salt);
        }

        /// <summary>
        /// Validates a password against a hash and salt using SHA512 algorithm.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <param name="salt">The salt used to hash the password.</param>
        /// <param name="hash">The hash of the password.</param>
        /// <returns>True if the password is valid, false otherwise.</returns>
        public bool ValidatePassword(string password, byte[] hash, byte[] salt)
        {
            // Hash the password and salt using SHA512
            using var sha512 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512);
            var testHash = sha512.GetBytes(HashSize);

            return CryptographicOperations.FixedTimeEquals(hash, testHash);
        }
    }
}
