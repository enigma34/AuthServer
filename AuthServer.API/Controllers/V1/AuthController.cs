using Asp.Versioning;
using AuthServer.API.DTOs;
using AuthServer.API.Models;
using AuthServer.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AuthServer.API.Controllers.V1
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private IUserServices _userServices;
        public AuthController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("register")]
        public ActionResult<RegisterResponse> Register(RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var allUsers = _userServices.GetAllUsers();
                User? checkuserexists = allUsers.Find(e => e.Email == request.Email);

                if (checkuserexists is not null)
                    return BadRequest("Email id already used");

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                User user = new User();
                user.Id = allUsers.Count() + 1;
                user.Email = request.Email;
                user.PasswordHash = passwordHash;
                user.USerRole = request.USerRole;
                _userServices.AddUser(user);
                return Ok(new RegisterResponse { Message= $"User: {user.Id} registered successfully!!!" });
            }
            catch (Exception ex)
            {
                Log.Error("Application error", ex.Message);
                return BadRequest("Internal Server Error");
            }
            
        }
    }
}
