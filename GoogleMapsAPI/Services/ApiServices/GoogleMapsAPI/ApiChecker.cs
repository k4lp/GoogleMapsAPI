using GoogleMapsAPI.Data;
using GoogleMapsAPI.Models;
using GoogleMapsAPI.Models.GoogleMapsApiServiceModels;
using GoogleMapsAPI.Services.HashingAndEncryptionService.EncryptionService;

using Newtonsoft.Json;

namespace GoogleMapsAPI.Services.ApiServices.GoogleMapsAPI
{
    public class ApiChecker : IApiChecker
    {
        private readonly HttpClient _httpClient;
        private readonly IEncryptionService _encryptionService;
        private readonly ApplicationDbContext _context;
        public ApiChecker(
            IEncryptionService encryptionService,
            HttpClient httpClient,
            ApplicationDbContext context
            )
        {
            _encryptionService = encryptionService;
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<bool> IsApiValidAsync(string ApiKey)
        {
            // Simplified null/empty check
            if (string.IsNullOrWhiteSpace(ApiKey))
                return false;
            //User? ifExist = await _context.Users!.FirstOrDefaultAsync(u => u.EncryptedApiKey == ApiKey);
            IEnumerable<User> var1 = _context.Users!.AsEnumerable();
            bool IsExist = var1.Any(u => _encryptionService.Decrypt(u.EncryptedApiKey!) == ApiKey);

            string url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=23.0225,72.5714&radius=10000&keyword=software-company&key=";
            string withApiKey = url + ApiKey;

            try
            {
                var response = await _httpClient.GetAsync(withApiKey);

                // Early return if response is null
                if (response == null)
                    return false;

                var rawBody = await response.Content.ReadAsStringAsync();
                var jsonBody = JsonConvert.DeserializeObject<GoogleMapsRoot>(rawBody);

                // Check if deserialization was successful and status is OK
                return jsonBody != null && jsonBody.Status?.Equals("OK", StringComparison.OrdinalIgnoreCase) == true;
            }
            catch (Exception)
            {
                // Consider logging the exception
                // _logger.LogError(ex, "API key validation failed");
                return false;
            }
        }
    }
}
