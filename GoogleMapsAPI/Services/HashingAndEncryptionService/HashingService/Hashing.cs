using Microsoft.IdentityModel.Tokens;

using System.Security.Cryptography;
using System.Text;

namespace GoogleMapsAPI.Services.HashingAndEncryptionService.HashingService
{
    public class HashingService : IHashingService
    {
        public string GenerateHash(string RawInput)
        {
            try
            {
                if (RawInput.IsNullOrEmpty())
                    throw new Exception("RawInput string cannot be empty. Try again.");
                using (var sha256 = SHA256.Create())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(RawInput);
                    byte[] hashBytes = sha256.ComputeHash(inputBytes);
                    return Convert.ToBase64String(hashBytes);
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public bool VerifyHash(string RawInput, string HashValue)
        {
            if (string.IsNullOrEmpty(RawInput) || string.IsNullOrEmpty(HashValue))
                throw new Exception("RawInput or HashValue is Empty. Try again.");

            return HashValue.Equals(GenerateHash(RawInput));
        }
    }
}
