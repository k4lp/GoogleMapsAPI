using GoogleMapsAPI.Extensions.ProgramServicesExtensions;
using GoogleMapsAPI.Middlewares.ExceptionHandlingMiddlewares;
using GoogleMapsAPI.Services.ApiServices.GoogleMapsAPI;

namespace GoogleMapsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            if (builder.Environment.IsDevelopment())
                builder.Configuration.AddUserSecrets<Program>();

            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddMyServices();


            //Injecting HTTPCLIENT in API CHECKER 
            builder.Services.AddHttpClient<IApiChecker, ApiChecker>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseExceptionHandlingMiddleware();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
