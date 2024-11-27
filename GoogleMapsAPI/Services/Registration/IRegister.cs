using GoogleMapsAPI.Models.Entities;

namespace GoogleMapsAPI.Services.Registration
{
    public interface IRegister
    {
        Task<bool> RegisterUserAsync(User user);
    }
}
