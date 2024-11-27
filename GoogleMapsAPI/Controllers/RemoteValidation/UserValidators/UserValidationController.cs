using GoogleMapsAPI.Data;
using GoogleMapsAPI.Services.ApiServices.GoogleMapsAPI;
using GoogleMapsAPI.Services.HashingAndEncryptionService.HashingService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GoogleMapsAPI.Controllers.RemoteValidation.UserValidators
{
    public class UserValidationController : Controller
    {
        private readonly IApiChecker _apiChecker;
        private readonly IHashingService _hashing;
        private readonly ApplicationDbContext _context;
        public UserValidationController(
            IApiChecker apiChecker,
            IHashingService hashing,
            ApplicationDbContext context)
        {
            _hashing = hashing;
            _apiChecker = apiChecker;
            _context = context;
        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> CheckApiKey(string EncryptedApiKey)
        {
            // Check if API key is null or empty
            if (string.IsNullOrWhiteSpace(EncryptedApiKey))
                return Json("Please enter a valid API key.");

            string hashedValue = _hashing.GenerateHash(EncryptedApiKey);

            bool doesExist = await _context.Users!.AnyAsync(u => u.ApiHash == hashedValue);
            if (doesExist)
                return Json("API key already exist.");
            try
            {
                // Validate the API key
                bool isValid = await _apiChecker.IsApiValidAsync(EncryptedApiKey);

                if (isValid)
                    return Json(true);
                else
                    return Json("The provided API key is invalid or cannot be verified.");
            }
            catch (Exception ex)
            {
                // Generic error handling
                return Json($"{ex.Message} : An error occurred while validating the API key.");
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> ValidateEmail(string email)
        {
            if (await _context.Users!.Where(u => u.Email!.ToLower() == email.ToLower()).AnyAsync())
                return Json("Email Already Exist. Use another email address.");
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckPassword(string password)
        {
            if (password == null)
                return Json("Please enter a password");
            if (password.Length < 8)
                return Json("Password must be 8 characters long");
            if (!password.Any(char.IsUpper))
                return Json("Password must contain an upper case letter");
            if (!password.Any(char.IsLower))
                return Json("Password must contain a lower case letter");
            if (!password.Any(char.IsDigit))
                return Json("Password must contain a number");

            var specialCharacters = "!@#$%^&*()_+-=[]{}|;:,.<>?";
            if (!password.Any(c => specialCharacters.Contains(c)))
            {
                return Json("Password must contain at least one special character");
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> CheckUsername(string username)
        {
            if (username.Length < 8 || username.Length > 20 || username.IsNullOrEmpty())
                return Json("Username must be 8 characters long and at most 20.");
            if (username.Any(char.IsWhiteSpace))
                return Json("Spaces are not allowed.");
            if (!char.IsLetter(username[0]))
                return Json("Username must start with a Letter");
            foreach (char c in username)
            {
                if (!char.IsLetterOrDigit(c))
                    return Json("Username can only contain letters and numbers");
            }
            if (await _context.Users!.Where(u => u.Username == username).AnyAsync())
                return Json("Username Already Exist.");
            return Json(true);
        }
    }
}
