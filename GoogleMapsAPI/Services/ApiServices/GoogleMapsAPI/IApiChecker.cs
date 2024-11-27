namespace GoogleMapsAPI.Services.ApiServices.GoogleMapsAPI
{
    public interface IApiChecker
    {
        Task<bool> IsApiValidAsync(string ApiKey);
    }
}
