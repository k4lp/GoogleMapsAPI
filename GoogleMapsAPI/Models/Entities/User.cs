using GoogleMapsAPI.Models.CustomValidationAttributes.UserValidationAttributes;

using System.ComponentModel.DataAnnotations;

namespace GoogleMapsAPI.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        //[StringLength(20, MinimumLength = 8, ErrorMessage = "Username must be between 8 and 20 characters")]
        //[Remote(action: "CheckUsername", controller: "UserValidation", ErrorMessage = "Please use proper username.")]
        [CustomUsernameValidation]
        public string? Username { get; set; }



        [Required]
        [DataType(DataType.Password)]
        public string? PasswordHash { get; set; }



        [Required]
        //[Remote(action: "CheckApiKey", controller: "UserValidation", ErrorMessage = "Enter a valid API key")]
        //[Display(Name = "API KEY for Google Maps", Description = "Your Google Maps API Key", Prompt = "Enter your API key")]
        public string? EncryptedApiKey { get; set; }

        [Required]
        public string? ApiHash { get; set; }


        [Required]
        [EmailAddress]
        //[Remote(action: "ValidateEmail", controller: "UserValidation", ErrorMessage = "Enter a valid email address")]
        [DataType(DataType.EmailAddress)]
        //[Display(Name = "Email Address", Description = "Your contact email", Prompt = "name@example.com")]
        public string? Email { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        //[Display(Name = "Phone Number", Description = "Your contact number", Prompt = "+91 12345-67890")]
        public string? Phone { get; set; }


        [DataType(DataType.DateTime)]
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        [DataType(DataType.DateTime)]
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
