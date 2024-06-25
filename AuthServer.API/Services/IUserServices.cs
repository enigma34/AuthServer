using AuthServer.API.Models;

namespace AuthServer.API.Services
{
    public interface IUserServices
    {
        public List<User> GetAllUsers();
    }
}
