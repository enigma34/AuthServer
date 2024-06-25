using System.ComponentModel.DataAnnotations;

namespace AuthServer.API.DTOs
{
    public class LoginRequest
    {
        [Required]
        [MinLength(8, ErrorMessage = "Email is must be atleast 7 characters")]
        [MaxLength(20, ErrorMessage = "Email is must be less than 20 characters")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20, ErrorMessage = "Password is must be less than 20 characters")]
        // [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",ErrorMessage = "Minimum 8 characters, At least  1 uppercase English letter, At least  1 lowercase English letter, At least 1 digit, At least 1 special character")]
        public string Password { get; set; } = string.Empty;
    }
}
