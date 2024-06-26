using AuthServer.API.DTOs;
using AuthServer.API.Models;
using System.Security.Claims;

namespace AuthServer.API.Services
{
    public interface IUserServices
    {
        public List<User> GetAllUsers();
        public User? GetUser(string email);
        public void AddUser(User user);
        public bool CheckUserInDb(string emailId);
        public int GetUsersCount();
        public LoginResponse CreateToken(User user);
        public bool ValidateToken(string token); 
        void RevokeToken(string token);
        bool IsTokenRevoked(string token);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        //public void SaveRefreshToken(string email, string refreshToken);
        public RefreshToken GetRefreshToken(string email);
        public void DeleteRefreshToken(string email);
    }
}
