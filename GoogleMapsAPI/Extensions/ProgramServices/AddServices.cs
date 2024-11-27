using GoogleMapsAPI.Services.ApiServices.GoogleMapsAPI;
using GoogleMapsAPI.Services.HashingAndEncryptionService.EncryptionService;
using GoogleMapsAPI.Services.HashingAndEncryptionService.HashingService;
using GoogleMapsAPI.Services.Registration;

namespace GoogleMapsAPI.Extensions.ProgramServices
{
    public static class AddServices
    {
        public static void AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IHashingService, HashingService>();
            services.AddScoped<IRegister, Register>();
            services.AddScoped<IApiChecker, ApiChecker>();
        }
    }
}
