using System.Security.Cryptography;
using System.Text;

namespace GoogleMapsAPI.Services.HashingAndEncryptionService.EncryptionService
{
    public class EncryptionService : IEncryptionService
    {
        public readonly string _encryptionKey;
        /// <summary>
        /// We need IConfiguration in order to access user secrets
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="Exception"></exception>
        public EncryptionService(IConfiguration configuration)
        {
            _encryptionKey = configuration["EncryptionSettings:EncryptionKey"] ?? throw new Exception("Encryption Key not found in user secrets");
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Plain text cannot be null or empty", nameof(plainText));

            byte[] saltBytes = GenerateRandomSalt();
            byte[] ivBytes = GenerateRandomIV();

            using (var password = new Rfc2898DeriveBytes(_encryptionKey, saltBytes, 20000, HashAlgorithmName.SHA256))
            {
                byte[] keyBytes = password.GetBytes(256 / 8);

                using (var aesAlg = Aes.Create())
                {
                    aesAlg.KeySize = 256;
                    aesAlg.Key = keyBytes;
                    aesAlg.IV = ivBytes;

                    using (var encryptor = aesAlg.CreateEncryptor())
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                        {
                            swEncrypt.Write(plainText);
                        }

                        // Concatenate salt + IV + ciphertext as raw bytes
                        byte[] encryptedBytes = msEncrypt.ToArray();
                        byte[] resultBytes = new byte[saltBytes.Length + ivBytes.Length + encryptedBytes.Length];

                        Buffer.BlockCopy(saltBytes, 0, resultBytes, 0, saltBytes.Length);
                        Buffer.BlockCopy(ivBytes, 0, resultBytes, saltBytes.Length, ivBytes.Length);
                        Buffer.BlockCopy(encryptedBytes, 0, resultBytes, saltBytes.Length + ivBytes.Length, encryptedBytes.Length);

                        // Convert to hexadecimal string
                        return BitConverter.ToString(resultBytes).Replace("-", ""); // Hexadecimal format
                    }
                }
            }
        }

        /// <summary>
        /// Decrypting the string.
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("Cipher text cannot be null or empty", nameof(cipherText));

            // Convert hex string back to bytes
            byte[] cipherBytes = Enumerable.Range(0, cipherText.Length / 2)
                                            .Select(x => Convert.ToByte(cipherText.Substring(x * 2, 2), 16))
                                            .ToArray();

            // Extract salt, IV, and encrypted data
            byte[] saltBytes = new byte[16];
            byte[] ivBytes = new byte[16];
            byte[] encryptedBytes = new byte[cipherBytes.Length - saltBytes.Length - ivBytes.Length];

            Buffer.BlockCopy(cipherBytes, 0, saltBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(cipherBytes, saltBytes.Length, ivBytes, 0, ivBytes.Length);
            Buffer.BlockCopy(cipherBytes, saltBytes.Length + ivBytes.Length, encryptedBytes, 0, encryptedBytes.Length);

            using (var password = new Rfc2898DeriveBytes(_encryptionKey, saltBytes, 20000, HashAlgorithmName.SHA256))
            {
                byte[] keyBytes = password.GetBytes(256 / 8);

                using (var aesAlg = Aes.Create())
                {
                    aesAlg.KeySize = 256;
                    aesAlg.Key = keyBytes;
                    aesAlg.IV = ivBytes;

                    using (var decryptor = aesAlg.CreateDecryptor())
                    using (var msDecrypt = new MemoryStream(encryptedBytes))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                    {
                        return srDecrypt.ReadToEnd(); // Plain text
                    }
                }
            }
        }
        // Utility methods
        private static byte[] GenerateRandomSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return saltBytes;
        }

        private static byte[] GenerateRandomIV()
        {
            byte[] ivBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(ivBytes);
            }
            return ivBytes;
        }
    }
}
