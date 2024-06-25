using AuthServer.API.Models;

namespace AuthServer.API.Services
{
    public class UserServices: IUserServices
    {
        private List<User> testUsers;
        public UserServices()
        {
            testUsers = new List<User>();
            testUsers.Add(new User() { Id = 1, Email = "abc@xyz.com", Password = "test", USerRole = new() { Roles.Admin.ToString(), Roles.User.ToString()} });
            testUsers.Add(new User() { Id = 2, Email = "123@xyz.com", Password = "test", USerRole = new() { Roles.User.ToString() } });
            testUsers.Add(new User() { Id = 3, Email = "wsad@xyz.com", Password = "test", USerRole = new() { Roles.User.ToString() } });
        }

        public List<User> GetAllUsers()
        {
                return testUsers;
        }
    }
}
