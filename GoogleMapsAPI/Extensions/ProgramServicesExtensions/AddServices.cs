using GoogleMapsAPI.Data;
using GoogleMapsAPI.Services.ApiServices.GoogleMapsAPI;
using GoogleMapsAPI.Services.HashingAndEncryptionService.EncryptionService;
using GoogleMapsAPI.Services.HashingAndEncryptionService.HashingService;
using GoogleMapsAPI.Services.Registration;

using Microsoft.EntityFrameworkCore;

namespace GoogleMapsAPI.Extensions.ProgramServicesExtensions
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

        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_connectionString));
        }
    }
}
