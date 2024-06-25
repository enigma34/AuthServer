using AuthServer.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthServer.API.Services
{
    public class UserServices: IUserServices
    {
        private List<User> testUsers;
        private readonly IConfiguration _configuration;
        private List<string> _revokedTokens;

        public UserServices(IConfiguration configuration)
        {
            //palin password "test"
            testUsers = new List<User>();
            testUsers.Add(new User() { Id = 1, Email = "abc@xyz.com", PasswordHash = "$2a$11$BDW8s0ctkCo35NqKfdXmG.aSME0Tqne6wepyeVYctpkeft2KStluC", USerRole = new() { Roles.Admin.ToString(), Roles.User.ToString()} });
            testUsers.Add(new User() { Id = 2, Email = "123@xyz.com", PasswordHash = "$2a$11$BDW8s0ctkCo35NqKfdXmG.aSME0Tqne6wepyeVYctpkeft2KStluC", USerRole = new() { Roles.User.ToString() } });
            testUsers.Add(new User() { Id = 3, Email = "wsad@xyz.com", PasswordHash = "$2a$11$BDW8s0ctkCo35NqKfdXmG.aSME0Tqne6wepyeVYctpkeft2KStluC", USerRole = new() { Roles.User.ToString() } });
            _configuration = configuration;
            _revokedTokens = new List<string>();
            //_revokedTokens.Add("eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhYmNAeHl6LmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJBZG1pbiIsIlVzZXIiXSwiZXhwIjoxNzE5MzI0NDYyfQ.reQaQXNU1GxS1j7eA_AXilJVGmvvv-vuzJkM6i0UaWs");
            //_revokedTokens.Add("eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhYmNAeHl6LmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJBZG1pbiIsIlVzZXIiXSwiZXhwIjoxNzE5MzMwNzA5fQ.uwnghM_4slQzeC-AkSszODlZqHc-j6Ca5vZse0GuA-0");
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

        public User? GetUser(string email)
        {
            try
            {
                User? user = testUsers.Find(e => e.Email == email);
                return user;
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

        public bool CheckUserInDb(string emailId)
        {
            try
            {
                var allUsers = GetAllUsers();
                bool checkuserexists = allUsers.Any(e => e.Email == emailId);
                return checkuserexists;
            }
            catch (Exception ex)
            {
                Log.Error("Application error", ex.Message);
                return false;
            }           
        }

        public int GetUsersCount()
        {
            return GetAllUsers().Count;
        }

        public string CreateToken(User user)
        {
            try
            {
                List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email)
            };
                foreach (var role in user.USerRole)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:Token").Value!));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: creds
                    );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
            catch (Exception ex)
            {
                Log.Error("Application error", ex.Message);
                return string.Empty;
            }            
        }

        public bool ValidateToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                    var expiry = jwtToken?.ValidTo;

                    if (expiry.HasValue && expiry.Value < DateTime.UtcNow)
                    {
                        return false;
                    }

                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                Log.Error("Application error", ex.Message);
                return false;
            }
        }

        public void RevokeToken(string token)
        {
            _revokedTokens.Add(token);
        }

        public bool IsTokenRevoked(string token)
        {
            return _revokedTokens.Contains(token);
        }
    }
}
