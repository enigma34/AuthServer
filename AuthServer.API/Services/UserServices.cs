using AuthServer.API.Models;
using Serilog;

namespace AuthServer.API.Services
{
    public class UserServices: IUserServices
    {
        private List<User> testUsers;
        public UserServices()
        {
            testUsers = new List<User>();
            testUsers.Add(new User() { Id = 1, Email = "abc@xyz.com", PasswordHash = "test", USerRole = new() { Roles.Admin.ToString(), Roles.User.ToString()} });
            testUsers.Add(new User() { Id = 2, Email = "123@xyz.com", PasswordHash = "test", USerRole = new() { Roles.User.ToString() } });
            testUsers.Add(new User() { Id = 3, Email = "wsad@xyz.com", PasswordHash = "test", USerRole = new() { Roles.User.ToString() } });
        }

        public List<User> GetAllUsers()
         {
            try
            {
                var users = testUsers.ToList();
                return users;
            }
            catch (Exception ex)
            {
                Log.Error("Application error", ex.Message);
                return null;
            }
                
        }

        public void AddUser(User user)
        {
            try
            {
                testUsers.Add(user);
            }
            catch (Exception ex)
            {
                Log.Error("Application error", ex.Message);
            }           
        }
    }
}
