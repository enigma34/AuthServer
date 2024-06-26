using System.ComponentModel.DataAnnotations;

namespace AuthServer.API.DTOs
{
    public class RefreshTokenRequest
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
