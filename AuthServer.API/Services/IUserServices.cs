using AuthServer.API.Models;

namespace AuthServer.API.Services
{
    public interface IUserServices
    {
        public List<User> GetAllUsers();
        public User? GetUser(string email);
        public void AddUser(User user);
        public bool CheckUserInDb(string emailId);
        public int GetUsersCount();
        public string CreateToken(User user);
        public bool ValidateToken(string token); 
        void RevokeToken(string token);
        bool IsTokenRevoked(string token);
    }
}
