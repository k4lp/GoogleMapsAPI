using GoogleMapsAPI.Models;

namespace GoogleMapsAPI.Services.Registration
{
    public interface IRegister
    {
        Task<bool> RegisterUserAsync(User user);
    }
}
