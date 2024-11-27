using GoogleMapsAPI.Data;
using GoogleMapsAPI.Models.Entities;
using GoogleMapsAPI.Services.HashingAndEncryptionService.EncryptionService;
using GoogleMapsAPI.Services.HashingAndEncryptionService.HashingService;

namespace GoogleMapsAPI.Services.Registration
{
    public class Register : IRegister
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IHashingService _hashing;
        private readonly ApplicationDbContext _context;
        public Register(
            ApplicationDbContext context,
            IHashingService hashing,
            IEncryptionService encryptionService
            )
        {
            _context = context;
            _encryptionService = encryptionService;
            _hashing = hashing;
        }
        public async Task<bool> RegisterUserAsync(User user)
        {
            try
            {
                user.ApiHash = _hashing.GenerateHash(user.EncryptedApiKey!);
                user.Password = _hashing.GenerateHash(user.Password!);
                user.EncryptedApiKey = _encryptionService.Encrypt(user.EncryptedApiKey!);

                string rawPhoneNumber = user.Phone!.Trim();
                string processedNumber = string.Empty;
                foreach (char phone in rawPhoneNumber)
                {
                    if (char.IsDigit(phone))
                        processedNumber += phone;
                }


                if (processedNumber.Length < 10)
                    throw new Exception("Phone Numbers cannot be less than 10 digits.");

                //only the last ten digits should be stored
                if (processedNumber.Length >= 11)
                    processedNumber = processedNumber.Substring(processedNumber.Length % 10, 10);

                user.Phone = processedNumber;
                await _context.Users!.AddAsync(user);

                await _context.SaveChangesAsync();

                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }
    }
}
