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
        private readonly ILogger<Register> _logger;
        private readonly ApplicationDbContext _context;
        public Register(
            ILogger<Register> logger,
            ApplicationDbContext context,
            IHashingService hashing,
            IEncryptionService encryptionService
            )
        {
            _logger = logger;
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
                _logger.LogInformation("In Register Service - Encrypted Data success");
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
                _logger.LogInformation($"Registration user {user.Username} Successful!");
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to register {user.Username} - {ex.Message}");
                return await Task.FromResult(false);
            }
        }
    }
}
