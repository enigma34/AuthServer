namespace AuthServer.API.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserEmailId { get; set; }
        public string AppRefreshToken { get; set; }
        public string AccessToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
