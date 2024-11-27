using System.ComponentModel.DataAnnotations;

namespace GoogleMapsAPI.Models.ViewModels.UserViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username", Prompt = "Enter your username")]
        public string? Username { get; set; }
        [Required]
        [Display(Name = "Password", Prompt = "Enter your password")]
        public string? Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; } = false;
    }
}
